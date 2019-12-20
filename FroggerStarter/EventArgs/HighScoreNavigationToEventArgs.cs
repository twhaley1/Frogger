using System.Collections.Generic;
using FroggerStarter.Model.HighScore;

namespace FroggerStarter.EventArgs
{
    /// <summary>
    ///     Defines a HighScoreNavigationToEventArgs.
    /// </summary>
    public class HighScoreNavigationToEventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the high scores.
        /// </summary>
        /// <value>
        ///     The high scores.
        /// </value>
        public IEnumerable<HighScoreData> HighScores { get; set; }

        #endregion
    }
}