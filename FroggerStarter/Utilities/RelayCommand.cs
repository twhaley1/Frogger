﻿using System;
using System.Windows.Input;

namespace FroggerStarter.Utilities
{
    /// <summary>
    ///     Holds functionality to decouple view and viewmodel.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand : ICommand
    {
        #region Data members

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand" /> class.
        ///     Precondition: None
        ///     Postcondition: this.execute == execute AND this.canExecute == canExecute
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        /// <returns>
        ///     true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            var result = this.canExecute?.Invoke(parameter) ?? true;
            return result;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.execute(parameter);
            }
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        /// <returns></returns>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Typically, protected but made public, so can trigger a manual refresh on the result of CanExecute.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, System.EventArgs.Empty);
        }

        #endregion
    }
}