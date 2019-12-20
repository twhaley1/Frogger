using FroggerStarter.Model.GameObjects;

namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines a MovableObjectEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class MovableObjectEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the rider.
        /// </summary>
        /// <value>
        ///     The rider.
        /// </value>
        public GameObject MovingObject { get; set; }

        #endregion
    }
}