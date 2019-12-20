using System;
using System.Collections.Generic;

namespace FroggerStarter.Model.HighScore
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error
    /// <summary>
    ///     Defines the properties and behavior of a NameScoreLevelComparer.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Comparer{FroggerStarter.Model.HighScore.HighScoreData}" />
    public class NameScoreLevelComparer : Comparer<HighScoreData>
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
    {
        #region Methods

        /// <summary>
        ///     Compares objects by their name, then by score, then by level.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A comparison value based on the sort order.</returns>
        public override int Compare(HighScoreData x, HighScoreData y)
        {
            if (x != null && y != null)
            {
                var nameComparison = string.Compare(x.Name.ToLower(), y.Name.ToLower(), StringComparison.Ordinal);
                if (nameComparison == 0)
                {
                    var scoreComparison = x.Score.CompareTo(y.Score);
                    if (scoreComparison == 0)
                    {
                        return -1 * x.Level.CompareTo(y.Level);
                    }

                    return -1 * scoreComparison;
                }

                return nameComparison;
            }

            return 0;
        }

        #endregion
    }
}