using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Collisions;
using FroggerStarter.Enums;
using FroggerStarter.EventArgs;
using FroggerStarter.View.Sprites;
using Point = Windows.Foundation.Point;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Defines basic properties and behavior of every game object.
    /// </summary>
    public abstract class GameObject
    {
        #region Types and Delegates

        /// <summary>
        ///     An operation done to the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public delegate void Operation(BaseSprite sprite);

        #endregion

        #region Data members

        private Point location;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x location of the game object.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => this.location.X;
            set
            {
                this.location.X = value;
                this.Render();
            }
        }

        /// <summary>
        ///     Gets or sets the y location of the game object.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => this.location.Y;
            set
            {
                this.location.Y = value;
                this.Render();
            }
        }

        /// <summary>
        ///     Gets the x speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed x.
        /// </value>
        public double SpeedX { get; private set; }

        /// <summary>
        ///     Gets the y speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed y.
        /// </value>
        public double SpeedY { get; private set; }

        /// <summary>
        ///     Gets the width of the game object.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.Sprite.Width;

        /// <summary>
        ///     Gets the height of the game object.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public double Height => this.Sprite.Height;

        /// <summary>
        ///     Gets or sets the sprite.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; protected set; }

        /// <summary>
        ///     Gets the animations.
        /// </summary>
        /// <value>
        ///     The animations.
        /// </value>
        public Dictionary<AnimationType, Animation> Animations { get; } = new Dictionary<AnimationType, Animation>();

        /// <summary>
        ///     Gets or sets the collision.
        /// </summary>
        /// <value>
        ///     The collision.
        /// </value>
        public Collision Collision { get; set; } = new Collision();

        #endregion

        #region Methods

        /// <summary>
        ///     Performs an operation on each animation sprite.
        ///     Precondition: None
        ///     Postcondition: specified operate applied to each animationSprite in this.Animations
        /// </summary>
        /// <param name="operate">The operate.</param>
        public void ForEachAnimationSprite(Operation operate)
        {
            foreach (var animationSprite in this.Animations.Values.SelectMany(animation => animation.AllSprites))
            {
                operate(animationSprite);
            }
        }

        /// <summary>
        ///     Moves the game object right.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public void MoveRight()
        {
            this.moveX(this.SpeedX);
        }

        /// <summary>
        ///     Moves the game object left.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public void MoveLeft()
        {
            this.moveX(-this.SpeedX);
        }

        /// <summary>
        ///     Moves the game object up.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev - SpeedY
        /// </summary>
        public void MoveUp()
        {
            this.moveY(-this.SpeedY);
        }

        /// <summary>
        ///     Moves the game object down.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev + SpeedY
        /// </summary>
        public void MoveDown()
        {
            this.moveY(this.SpeedY);
        }

        private void moveX(double x)
        {
            this.X += x;
        }

        private void moveY(double y)
        {
            this.Y += y;
        }

        /// <summary>
        ///     Renders this instance.
        /// </summary>
        protected virtual void Render()
        {
            this.Sprite.RenderAt(this.X, this.Y);
            this.ForEachAnimationSprite(sprite => sprite.RenderAt(this.X, this.Y));
        }

        /// <summary>
        ///     Sets the speed of the game object.
        ///     Precondition: speedX >= 0 AND speedY >=0
        ///     Postcondition: SpeedX == speedX AND SpeedY == speedY
        /// </summary>
        /// <param name="speedX">The speed x.</param>
        /// <param name="speedY">The speed y.</param>
        protected void SetSpeed(double speedX, double speedY)
        {
            if (speedX < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speedX));
            }

            if (speedY < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speedY));
            }

            this.SpeedX = speedX;
            this.SpeedY = speedY;
        }

        /// <summary>
        ///     Determines whether [is colliding with] [the specified other]. The other must have its Visibility set to Visible.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if [is colliding with] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsCollidingWith(GameObject other)
        {
            if (other.Sprite.Visibility == Visibility.Collapsed)
            {
                return false;
            }

            if (other.Animations.ContainsKey(AnimationType.Death) && other.Animations[AnimationType.Death].IsAnimating)
            {
                return false;
            }

            var collider1 = new Rectangle(
                (int) this.X, (int) this.Y, (int) this.Width, (int) this.Height);
            var collider2 = new Rectangle(
                (int) other.X, (int) other.Y, (int) other.Width, (int) other.Height);

            return collider1.IntersectsWith(collider2);
        }

        /// <summary>
        ///     Occurs when [moved].
        /// </summary>
        public event EventHandler<MovableObjectEventArgs> Moved;

        /// <summary>
        ///     Invokes this GameObject's Moved event.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        protected virtual void onMoved()
        {
            var arg = new MovableObjectEventArgs {MovingObject = this};
            this.Moved?.Invoke(this, arg);
        }

        #endregion
    }
}