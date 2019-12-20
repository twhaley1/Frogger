namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines properties and behavior of a PlayAgainNavigationToEventArgs.
    /// </summary>
    public class PlayAgainNavigationToEventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public int Level { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

        #endregion
    }
}