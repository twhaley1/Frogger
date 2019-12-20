using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines basic properties and behavior of every animation.
    /// </summary>
    public class Animation
    {
        #region Data members

        private readonly BaseSprite defaultFrame;
        private readonly int animationTime;
        private readonly Queue<BaseSprite> frames;
        private BaseSprite currentFrame;
        private DispatcherTimer animationTimer;
        private int timesAnimationTicked;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether this instance is animating.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is animating; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnimating { get; private set; }

        /// <summary>
        ///     Gets all sprites.
        /// </summary>
        /// <value>
        ///     All sprites.
        /// </value>
        public IEnumerable<BaseSprite> AllSprites => this.frames;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Animation" /> class with animationTime in milliseconds.
        ///     Precondition: animationTime &gt;= 0 AND defaultSprite != null
        ///     Postcondition: this.defaultFrame == defaultSprite
        ///     this.currentFrame == defaultSprite
        ///     this.frames == an empty Queue of type BaseSprite
        ///     this.animationTime == animationTime
        ///     this.timesAnimationTicked = 0
        /// </summary>
        /// <param name="defaultSprite">The default sprite.</param>
        /// <param name="animationTime">The animation time in milliseconds.</param>
        /// <exception cref="ArgumentNullException">defaultFrame</exception>
        public Animation(BaseSprite defaultSprite, int animationTime)
        {
            if (animationTime < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(animationTime));
            }

            this.defaultFrame = defaultSprite ?? throw new ArgumentNullException(nameof(defaultSprite));
            this.currentFrame = defaultSprite;
            this.frames = new Queue<BaseSprite>();
            this.animationTime = animationTime;
            this.timesAnimationTicked = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Animation" /> class.
        ///     Precondition: frames != null
        ///     Postcondition: this.defaultFrame == defaultSprite
        ///     this.currentFrame == defaultSprite
        ///     this.frames == a Queue of type BaseSprite containing the specified frames
        ///     this.animationTime == animationTime
        ///     this.timesAnimationTicked = 0
        /// </summary>
        /// <param name="defaultSprite">The default sprite.</param>
        /// <param name="animationTime">The animation time.</param>
        /// <param name="frames">The frames.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Animation(BaseSprite defaultSprite, int animationTime, Queue<BaseSprite> frames) : this(defaultSprite,
            animationTime)
        {
            this.frames = frames ?? throw new ArgumentNullException();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when the animation begins.
        /// </summary>
        public event EventHandler AnimationStarted;

        /// <summary>
        ///     Occurs when the animation ends.
        /// </summary>
        public event EventHandler AnimationEnded;

        /// <summary>
        ///     Plays the animation by cycling through the frames, unless it is currently animating.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public void DoAnimation()
        {
            if (!this.IsAnimating)
            {
                this.onAnimationStarted();
                this.IsAnimating = true;
                this.setupAnimationTimer();
            }
        }

        private void setupAnimationTimer()
        {
            this.animationTimer = new DispatcherTimer();
            this.animationTimer.Tick += this.animationTimerOnTick;
            var timePerFrame = this.animationTime / this.frames.Count;
            this.animationTimer.Interval = new TimeSpan(0, 0, 0, 0, timePerFrame);
            this.animationTimer.Start();
        }

        private void animationTimerOnTick(object sender, object e)
        {
            this.timesAnimationTicked++;
            this.currentFrame.Visibility = Visibility.Collapsed;
            if (this.frames.Count >= this.timesAnimationTicked)
            {
                this.advanceFrame();
            }
            else
            {
                this.resetToDefaultFrame();
            }
        }

        private void resetToDefaultFrame()
        {
            this.timesAnimationTicked = 0;
            this.currentFrame = this.defaultFrame;
            this.defaultFrame.Visibility = Visibility.Visible;
            this.IsAnimating = false;
            this.animationTimer.Stop();
            this.onAnimationEnded();
        }

        private void advanceFrame()
        {
            this.currentFrame = this.frames.Dequeue();
            this.frames.Enqueue(this.currentFrame);
            this.currentFrame.Visibility = Visibility.Visible;
        }

        private void onAnimationStarted()
        {
            this.AnimationStarted?.Invoke(this, System.EventArgs.Empty);
        }

        private void onAnimationEnded()
        {
            this.AnimationEnded?.Invoke(this, System.EventArgs.Empty);
        }

        #endregion
    }
}