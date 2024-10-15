// ----------------------------------------------------------------------
// <copyright file="MainModel.cs" company="noName">
//     Copyright (c) noName s. r. o..  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------
namespace noName.WpfApp.Commands
{
    using System;
    using System.Windows.Input;


    /// <summary>
    /// Převedení Commandu tlačítka z GUI na metodu 
    /// </summary>
    public class RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand
    {
        #region Members

        private readonly Action<object?> _execute = execute;
        private readonly Predicate<object?>? _canExecute = canExecute;

        #endregion

        #region Events

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region ICommand

        /// <inheritdoc/>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <inheritdoc/>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        #endregion
    }

}
