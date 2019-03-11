#region Header

// Author 
// Created 08

#endregion

namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;

    #endregion

    [ContentProperty(nameof(Commands))]
    public class CommandGroup : DependencyObject, ICommand
    {
        #region  Fields

        private ObservableCollection<ICommand> _commands;

        #endregion

        #region Properties

        /// <summary>
        ///     Returns the collection of child commands. They are executed
        ///     in the order that they exist in this collection.
        /// </summary>
        public ObservableCollection<ICommand> Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = new ObservableCollection<ICommand>();
                    _commands.CollectionChanged += OnCommandsCollectionChanged;
                }

                return _commands;
            }
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            foreach (var cmd in Commands)
                if (!cmd.CanExecute(parameter))
                    return false;

            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            foreach (var cmd in Commands)
                cmd.Execute(parameter);
        }

        #endregion

        #region

        protected virtual void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        private void OnChildCommandCanExecuteChanged(object sender, EventArgs e)
        {
            // Bubble up the child commands CanExecuteChanged event so that
            // it will be observed by WPF.
            OnCanExecuteChanged();
        }

        private void OnCommandsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // We have a new child command so our ability to execute may have changed.
            OnCanExecuteChanged();

            if (e.NewItems != null && 0 < e.NewItems.Count)
                foreach (ICommand cmd in e.NewItems)
                    cmd.CanExecuteChanged += OnChildCommandCanExecuteChanged;

            if (e.OldItems != null && 0 < e.OldItems.Count)
                foreach (ICommand cmd in e.OldItems)
                    cmd.CanExecuteChanged -= OnChildCommandCanExecuteChanged;
        }

        #endregion
    }
}