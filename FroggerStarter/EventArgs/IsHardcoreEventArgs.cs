namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Manages a data value that tells whether to put the game into hardcore mode or not.
    /// </summary>
    public class IsHardcoreEventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is hardcore.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is hardcore; otherwise, <c>false</c>.
        /// </value>
        public bool IsHardcore { get; set; }

        #endregion
    }
}