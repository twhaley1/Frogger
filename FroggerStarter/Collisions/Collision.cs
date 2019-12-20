using System.Collections.Generic;
using System.Linq;

namespace FroggerStarter.Collisions
{
    /// <summary>
    ///     Defines the properties and behavior of a Collision.
    /// </summary>
    public class Collision
    {
        #region Types and Delegates

        /// <summary>
        ///     An Action that does not take a parameter.
        /// </summary>
        public delegate void NoParamAction();

        /// <summary>
        ///     A Predicate that does not take a parameter.
        /// </summary>
        /// <returns>true if the predicate is true, false otherwise.</returns>
        public delegate bool NoParamPredicate();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the on collision.
        /// </summary>
        /// <value>
        ///     The on collision.
        /// </value>
        public NoParamAction OnCollision { private get; set; }

        /// <summary>
        ///     Gets or sets the on no collision.
        /// </summary>
        /// <value>
        ///     The on no collision.
        /// </value>
        public NoParamAction OnNoCollision { private get; set; }

        /// <summary>
        ///     Gets or sets the determining factors.
        /// </summary>
        /// <value>
        ///     The determining factors.
        /// </value>
        public IEnumerable<NoParamPredicate> DeterminingFactors { private get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Processes the collision. If all determining factors return true, then the OnCollision method executes.
        ///     If one of the determining factors returns false, then the OnNoCollision method executes.
        /// </summary>
        /// <returns>true if all all determining factors return true, false otherwise.</returns>
        public bool ProcessCollision()
        {
            if (this.allTrueDeterminingFactors())
            {
                this.OnCollision?.Invoke();
                return true;
            }

            this.OnNoCollision?.Invoke();
            return false;
        }

        private bool allTrueDeterminingFactors()
        {
            foreach (var determiningFactor in this.DeterminingFactors ?? Enumerable.Empty<NoParamPredicate>())
            {
                if (!determiningFactor())
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}