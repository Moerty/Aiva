﻿using System;
using System.Windows.Input;

namespace Aiva.Bot.Internal {
    // http://stackoverflow.com/questions/22285866/why-relaycommand
    public class RelayCommand<T> : ICommand {
        #region Fields

        private readonly Action<T> _execute = null;
        private readonly Predicate<T> _canExecute = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
        public RelayCommand(Action<T> execute)
            : this(execute, null) {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute) {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        ///<returns>
        ///true if this command can be executed; otherwise, false.
        ///</returns>
        public bool CanExecute(object parameter) {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        ///<summary>
        ///Occurs when changes occur that affect whether or not the command should execute.
        ///</summary>
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter) {
            _execute((T)parameter);
        }

        #endregion ICommand Members
    }

    public class RelayCommand : ICommand {
        #region Fields

        private Action<object> execute;
        private Func<object, bool> canExecute;

        #endregion Fields

        #region Constructors

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute) {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) {
            this.execute = execute;
        }

        #endregion Constructors

        #region Members

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter) {
            this.execute(parameter);
        }

        #endregion Members
    }
}