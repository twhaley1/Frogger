using System;
using System.Runtime.Serialization;

namespace FroggerStarter.Model.HighScore
{
    /// <summary>
    ///     Defines the properties and behavior of a HighScoreData.
    /// </summary>
    [Serializable]
    [DataContract]
    public class HighScoreData : IComparable<HighScoreData>
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        [DataMember]
        public int Score { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        [DataMember]
        public int Level { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Compares objects by their score, then by name, then by level.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>A comparison value based on the sort order.</returns>
        public int CompareTo(HighScoreData other)
        {
            var scoreComparison = this.Score.CompareTo(other.Score);
            if (scoreComparison == 0)
            {
                var nameComparison =
                    string.Compare(this.Name.ToLower(), other.Name.ToLower(), StringComparison.Ordinal);
                if (nameComparison == 0)
                {
                    return -1 * this.Level.CompareTo(other.Level);
                }

                return nameComparison;
            }

            return -1 * scoreComparison;
        }

        #endregion
    }
}