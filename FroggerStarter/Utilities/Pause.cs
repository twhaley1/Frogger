using System;
using System.Threading.Tasks;

namespace FroggerStarter.Utilities
{
    /// <summary>
    ///     Contains utility for pausing program execution.
    /// </summary>
    public class Pause
    {
        #region Methods

        /// <summary>
        ///     Pauses execution for the specified number of milliseconds.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        public static async Task Milliseconds(int milliseconds)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
        }

        #endregion
    }
}