namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines an UpdateInvulnCountdownEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UpdateInvulnCountdownEventArgs : System.EventArgs
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