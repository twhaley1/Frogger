using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.Enums;
using FroggerStarter.EventArgs;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.Frog;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Defines the properties and behavior of a Frog.
    /// </summary>
    /// <seealso cref="GameObject" />
    public class Frog : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 50;
        private const int SpeedYDirection = 50;

        private DirectionType facingDirection;
        private DispatcherTimer invulnTimer;
        private int timesCountdownTicked;
        private bool isInvulnerable;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the lives.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

        /// <summary>
        ///     Gets or sets the invulnerability.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is invulnerable; otherwise, <c>false</c>.
        /// </value>
        public bool IsInvulnerable
        {
            get => this.isInvulnerable;
            set
            {
                this.isInvulnerable = value;
                if (this.isInvulnerable)
                {
                    this.setupInvulnTimer();
                }
            }
        }

        private DirectionType FacingDirection
        {
            get => this.facingDirection;
            set
            {
                if (SpeedX > 0 && SpeedY > 0)
                {
                    this.facingDirection = value;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Frog" /> class.
        ///     Precondition: lives &gt; 0
        ///     Postcondition: this.SpeedX == SpeedXDirection AND this.SpeedY == SpeedYDirection
        ///     this.facingDirection == DirectionType.Up
        ///     Sprites initialized with a FrogSprite and each frame of its death animation and movement animation
        ///     this.Sprite.CenterPoint for each sprite == the center of each sprite, instead of the top left corner
        ///     <param name="lives">The frog's starting number of lives.</param>
        /// </summary>
        public Frog(int lives)
        {
            this.setupProperties(lives < 0 ? 0 : lives);

            this.setupDeathAnimation();
            this.setupMovementAnimation();

            this.setupRotationVector();

            this.StartMovement();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Does base collision check unless the Frog is currently in a death animation.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if [is colliding with] [the specified other] AND not in a death animation; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsCollidingWith(GameObject other)
        {
            if (!Animations[AnimationType.Death].IsAnimating)
            {
                return base.IsCollidingWith(other);
            }

            return false;
        }

        /// <summary>
        ///     Decrements lives by 1.
        ///     Precondition: None
        ///     Postcondition: this.Lives == this.Lives @prev -1
        /// </summary>
        public void LoseLife()
        {
            this.Lives--;
        }

        /// <summary>
        ///     Adds the specified score times the specified multiplier to the overall score.
        ///     Precondition: None
        ///     Postcondition: this.Score == this.Score @prev + (scoreToAdd * multiplier)
        /// </summary>
        /// <param name="scoreToAdd">The score to add.</param>
        /// <param name="multiplier">The multiplier.</param>
        public void AddScore(int scoreToAdd, int multiplier)
        {
            this.Score += scoreToAdd * multiplier;
        }

        #endregion

        #region Movement

        /// <summary>
        ///     Starts the movement of the frog based on SpeedXDirection and SpeedYDirection.
        ///     Precondition: none
        ///     Postcondition: this.SpeedX == SpeedXDirection AND this.SpeedY == SpeedYDirection
        /// </summary>
        public void StartMovement()
        {
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        /// <summary>
        ///     Stops the frog from moving.
        ///     Precondition: none
        ///     Postcondition: this.SpeedX == 0 AND this.SpeedY == 0
        /// </summary>
        public void StopMovement()
        {
            SetSpeed(0, 0);
        }

        /// <summary>
        ///     Moves the left with boundary check.
        ///     Precondition: none
        ///     Postcondition: X -= Speed unless boundary hit, then no change
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>True if movement executed, false otherwise.</returns>
        public bool MoveLeftWithBoundaryCheck(double offset)
        {
            if (X > 0 + offset)
            {
                this.FacingDirection = DirectionType.Left;
                MoveLeft();
                this.handleFrogMovementBehavior();
                onMoved();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Moves the right with boundary check.
        ///     Precondition: none
        ///     Postcondition: X += Speed unless boundary hit, then no change
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>True if movement executed, false otherwise.</returns>
        public bool MoveRightWithBoundaryCheck(double offset)
        {
            if (X < GameManager.BackgroundWidth - Width - offset)
            {
                this.FacingDirection = DirectionType.Right;
                MoveRight();
                this.handleFrogMovementBehavior();
                onMoved();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Moves up with boundary check.
        ///     Precondition: none
        ///     Postcondition: Y -= Speed unless boundary hit, then no change
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void MoveUpWithBoundaryCheck(double offset)
        {
            if (Y > offset)
            {
                this.FacingDirection = DirectionType.Up;
                MoveUp();
                this.handleFrogMovementBehavior();
                onMoved();
            }
        }

        /// <summary>
        ///     Moves down with boundary check.
        ///     Precondition: none
        ///     Postcondition: Y += Speed unless boundary hit, then no change
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>True if movement executed, false otherwise.</returns>
        public bool MoveDownWithBoundaryCheck(double offset)
        {
            if (Y < offset - Height)
            {
                this.FacingDirection = DirectionType.Down;
                MoveDown();
                this.handleFrogMovementBehavior();
                onMoved();
                return true;
            }

            return false;
        }

        private void handleFrogMovementBehavior()
        {
            if (!Animations[AnimationType.Death].IsAnimating)
            {
                this.rotateSprite();
                Animations[AnimationType.Movement].DoAnimation();
            }
        }

        private void rotateSprite()
        {
            switch (this.FacingDirection)
            {
                case DirectionType.Up:
                    Sprite.Rotation = 0;
                    ForEachAnimationSprite(sprite => sprite.Rotation = 0);
                    break;
                case DirectionType.Right:
                    Sprite.Rotation = 90;
                    ForEachAnimationSprite(sprite => sprite.Rotation = 90);
                    break;
                case DirectionType.Down:
                    Sprite.Rotation = 180;
                    ForEachAnimationSprite(sprite => sprite.Rotation = 180);
                    break;
                case DirectionType.Left:
                    Sprite.Rotation = 270;
                    ForEachAnimationSprite(sprite => sprite.Rotation = 270);
                    break;
            }
        }

        #endregion

        #region Invulnerability

        /// <summary>
        ///     Ends the invulnerability.
        ///     Precondition: None
        ///     Postcondition: this.IsInvulnerable == false
        /// </summary>
        public void ClearInvulnerabilityEffect()
        {
            this.onInvulnCountdownEnded();
            this.IsInvulnerable = false;
        }

        private void setupInvulnTimer()
        {
            this.invulnTimer = new DispatcherTimer();
            this.invulnTimer.Tick += this.invulnTimerOnTick;
            this.invulnTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.invulnTimer.Start();
            this.onInvulnCountdownStarted();
        }

        private void invulnTimerOnTick(object sender, object e)
        {
            this.timesCountdownTicked++;
            this.updateInvulnCountdown();
            this.checkForInvulnCountdownExpire();
        }

        private void checkForInvulnCountdownExpire()
        {
            if (this.timesCountdownTicked == GameSettings.InvulnTimeInSeconds)
            {
                this.invulnTimer.Stop();
                this.ClearInvulnerabilityEffect();
            }
        }

        #endregion

        #region Helpers

        private void setupProperties(int lives)
        {
            if (lives <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lives));
            }

            this.Lives = lives;
            this.Score = 0;
            Sprite = new FrogSprite();
            this.FacingDirection = DirectionType.Up;
        }

        private void setupRotationVector()
        {
            var rotationOriginVector = new Vector3((float) (Width / 2), (float) (Height / 2), 0);
            Sprite.CenterPoint = rotationOriginVector;
            ForEachAnimationSprite(sprite => sprite.CenterPoint = rotationOriginVector);
        }

        private void setupDeathAnimation()
        {
            var frames = new List<BaseSprite> {
                new FrogDeathFrameOne(),
                new FrogDeathFrameTwo(),
                new FrogDeathFrameThree(),
                new FrogDeathFrameFour()
            };
            var deathAnimation = new Animation(Sprite, 1000, new Queue<BaseSprite>(frames));
            Animations.Add(AnimationType.Death, deathAnimation);
        }

        private void setupMovementAnimation()
        {
            var frames = new List<BaseSprite> {
                new FrogMovementSprite()
            };
            var movementAnimation = new Animation(Sprite, 45, new Queue<BaseSprite>(frames));
            Animations.Add(AnimationType.Movement, movementAnimation);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when [invuln countdown started].
        /// </summary>
        public event EventHandler InvulnCountdownStarted;

        /// <summary>
        ///     Occurs when [invuln countdown ended].
        /// </summary>
        public event EventHandler InvulnCountDownEnded;

        /// <summary>
        ///     Occurs when [update invuln countdown event arguments].
        /// </summary>
        public event EventHandler<UpdateInvulnCountdownEventArgs> UpdateInvulnCountdown;

        private void onInvulnCountdownStarted()
        {
            this.InvulnCountdownStarted?.Invoke(this, System.EventArgs.Empty);
        }

        private void onInvulnCountdownEnded()
        {
            this.InvulnCountDownEnded?.Invoke(this, System.EventArgs.Empty);
        }

        private void updateInvulnCountdown()
        {
            var updateInvulnCountdownEvent = new UpdateInvulnCountdownEventArgs
                {TimesTicked = this.timesCountdownTicked};
            this.UpdateInvulnCountdown?.Invoke(this, updateInvulnCountdownEvent);
        }

        #endregion
    }
}