namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines an UpdateScoreEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UpdateScoreEventArgs : System.EventArgs
    {
        #region Properties

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