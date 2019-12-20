using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Obstacle;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Models a LilyPad Platform.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObjects.Platform" />
    public class LilyPad : Platform
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LilyPad" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Sprite == a LilyPadSprite
        ///     this.Direction == direction
        /// </summary>
        /// <param name="direction">The direction of the vehicle.</param>
        public LilyPad(DirectionType direction) : base(direction)
        {
            Sprite = new LilyPadSprite();
            SetDirection();
        }

        #endregion
    }
}