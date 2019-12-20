using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FroggerStarter.Model.HighScore
{
    /// <summary>
    ///     Defines the properties and behavior of a HighScores.
    /// </summary>
    public class HighScores : IEnumerable<HighScoreData>
    {
        #region Data members

        private readonly List<HighScoreData> highScores;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScores" /> class.
        ///     Precondition: None
        ///     Postcondition: this.highScores is populated with the specified highScores
        /// </summary>
        /// <param name="highScores">The high scores.</param>
        public HighScores(IEnumerable<HighScoreData> highScores)
        {
            this.highScores = new List<HighScoreData>();
            this.addAll(highScores);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<HighScoreData> GetEnumerator()
        {
            return this.highScores.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        ///     Sorts in the default ordering.
        ///     Precondition: None
        ///     Postcondition: this.highScores is sorted by the default ordering
        /// </summary>
        public void DefaultSort()
        {
            this.highScores.Sort();
        }

        /// <summary>
        ///     Sorts by the specified comparer.
        ///     Precondition: None
        ///     Postcondition: this.highScores is sorted by the specified comparer
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public void Sort(IComparer<HighScoreData> comparer)
        {
            this.highScores.Sort(comparer);
        }

        private void addAll(IEnumerable<HighScoreData> data)
        {
            foreach (var highScoreData in data ?? Enumerable.Empty<HighScoreData>())
            {
                if (highScoreData != null)
                {
                    this.highScores.Add(highScoreData);
                }
            }
        }

        #endregion
    }
}