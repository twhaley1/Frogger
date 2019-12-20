namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines an UpdateLivesEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UpdateLivesEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the lives.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives { get; set; }

        #endregion
    }
}