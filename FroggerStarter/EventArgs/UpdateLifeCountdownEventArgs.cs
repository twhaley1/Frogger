namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines a UpdateLifeCountdownEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UpdateLifeCountdownEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the times ticked.
        /// </summary>
        /// <value>
        ///     The times ticked.
        /// </value>
        public int TimesTicked { get; set; }

        #endregion
    }
}