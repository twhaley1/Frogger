namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines a GameOverEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class GameOverEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is game over.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is game over; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameOver { get; set; }

        #endregion
    }
}