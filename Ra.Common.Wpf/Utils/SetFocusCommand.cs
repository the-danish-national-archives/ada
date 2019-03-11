namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    #endregion

    /// <summary>
    ///     Sets focus to the element specified as a command parameter
    ///     It has the Instance property so that it can be used without instatiation from Xaml
    /// </summary>
    public class SetFocusCommand : ICommand
    {
        #region Static

        private static ICommand _instance;

        #endregion

        #region  Constructors

        static SetFocusCommand()
        {
            if (Application.Current == null) new Application();
        }

        #endregion

        #region Properties

        public static ICommand Instance => _instance ?? (_instance = new SetFocusCommand());

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
//            return parameter is DependencyObject && parameter is IInputElement;
            return parameter is IInputElement;
        }

        public void Execute(object parameter)
        {
            // Experiments with setting focus from FocusManager or as inputElement or the .focus is the cause for outcommented code
            Application.Current.Dispatcher.Invoke(() =>
            {
//                var dependencyObject = parameter as DependencyObject;
                var inputElement = parameter as IInputElement;
                if (inputElement != null)
//                if (dependencyObject != null && inputElement != null)
                {
                    // Because of problems with keyboard navigation when setting focus on 
                    // ListBox after the first time, the selected item will be reset each time
                    ListBox listBox;
                    if ((listBox = parameter as ListBox) != null)
                    {
                        listBox.Focus();
                        listBox.SelectedIndex = 0;
                        var item = listBox.ItemContainerGenerator.ContainerFromIndex(0);
                        ((ListBoxItem) item).Focus();
//                        }
                        return;
                    }


//                    var scope = FocusManager.GetFocusScope(dependencyObject);
//                    Console.WriteLine($"scope={scope}");
//                    IInputElement focusedElement = FocusManager.GetFocusedElement(scope);
//                    Console.WriteLine($"focus={focusedElement}");
//                    Console.WriteLine($"inputElement.IsKeyboardFocusWithin={inputElement.IsKeyboardFocusWithin}");
//                    Console.WriteLine($"inputElement.IsKeyboardFocused={inputElement.IsKeyboardFocused}");

                    if (inputElement.IsKeyboardFocusWithin) return;

                    inputElement.Focus();
                    Keyboard.Focus(inputElement);
                }
            });
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}