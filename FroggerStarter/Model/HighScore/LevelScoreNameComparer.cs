using System;
using System.Collections.Generic;

namespace FroggerStarter.Model.HighScore
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error
    /// <summary>
    ///     Defines the properties and behavior of a LevelScoreNameComparer.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Comparer{FroggerStarter.Model.HighScore.HighScoreData}" />
    public class LevelScoreNameComparer : Comparer<HighScoreData>
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
    {
        #region Methods

        /// <summary>
        ///     Compares objects by their level, then by score, then by name.
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
                var levelComparison = x.Level.CompareTo(y.Level);
                if (levelComparison == 0)
                {
                    var scoreComparison = x.Score.CompareTo(y.Score);
                    if (scoreComparison == 0)
                    {
                        return string.Compare(x.Name.ToLower(), y.Name.ToLower(), StringComparison.Ordinal);
                    }

                    return -1 * scoreComparison;
                }

                return -1 * levelComparison;
            }

            return 0;
        }

        #endregion
    }
}