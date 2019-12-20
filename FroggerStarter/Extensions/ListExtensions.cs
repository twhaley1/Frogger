using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FroggerStarter.Extensions
{
    /// <summary>
    ///     Extension methods for Lists.
    /// </summary>
    public static class ListExtensions
    {
        #region Methods

        /// <summary>
        ///     Converts to observable collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The specified IEnumerable as an ObservableCollection.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        #endregion
    }
}