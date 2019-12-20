using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Windows.UI.Xaml;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.Home;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     A home that the player lands in.
    /// </summary>
    /// <seealso cref="GameObject" />
    public class Home : GameObject
    {
        #region Data members

        private readonly BaseSprite filledSprite;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether the home is full.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this home is full; otherwise, <c>false</c>.
        /// </value>
        public bool IsFull => this.filledSprite.Visibility == Visibility.Visible;

        /// <summary>
        ///     Gets all sprites.
        /// </summary>
        /// <value>
        ///     All sprites.
        /// </value>
        public IEnumerable<BaseSprite> AllSprites { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Home" /> class.
        ///     Precondition: none
        ///     Postcondition: this.Sprite == a HomeEmptySprite
        ///     this.filledSprite == an invisible HomeFullSprite
        ///     this.AllSprites == a collection of type BaseSprite containing this.Sprite and this.filledSprite
        /// </summary>
        public Home()
        {
            Sprite = new HomeEmptySprite();
            this.filledSprite = new HomeFullSprite {Visibility = Visibility.Collapsed};
            this.AllSprites = new Collection<BaseSprite> {Sprite, this.filledSprite};
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Determines whether [is colliding with] [the specified other]. The other must have its Visibility set to Visible.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if [is colliding with] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsCollidingWith(GameObject other)
        {
            var homeCollision = false;
            if (this.filledSprite.Visibility != Visibility.Visible)
            {
                homeCollision = this.checkForCollision(other);
            }

            if (homeCollision)
            {
                this.FillHome();
            }

            return homeCollision;
        }

        private bool checkForCollision(GameObject other)
        {
            var cushion = Width * .45;

            var collider1 = new Rectangle(
                (int) (X + cushion), (int) Y, (int) (Width - 2 * cushion),
                (int) Height);
            var collider2 = new Rectangle(
                (int) other.X, (int) other.Y, (int) other.Width, (int) other.Height);

            return collider1.IntersectsWith(collider2);
        }

        /// <summary>
        ///     Clears the home.
        ///     Precondition: None
        ///     Postcondition: this.Sprite.Visibility == Visibility.Visible
        ///     this.filledSprite.Visibility == Visibility.Collapsed
        /// </summary>
        public void ClearHome()
        {
            Sprite.Visibility = Visibility.Visible;
            this.filledSprite.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Fills the home if it is empty.
        ///     Precondition: none
        ///     Postcondition: if this.IsFull == true, then:
        ///     this.Sprite.Visibility = Visibility.Collapsed
        ///     this.filledSprite.Visibility = Visibility.Visible
        /// </summary>
        public void FillHome()
        {
            if (!this.IsFull)
            {
                Sprite.Visibility = Visibility.Collapsed;
                this.filledSprite.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}