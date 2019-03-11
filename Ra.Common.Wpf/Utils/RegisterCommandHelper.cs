namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Input;

    #endregion

    public static class RegisterCommandHelper
    {
        #region Static

        // from https://codingcontext.wordpress.com/2008/12/10/commandbindings-in-mvvm/

        public static readonly DependencyProperty RegisterCommandBindingsProperty =
            DependencyProperty.RegisterAttached(
                "RegisterCommandBindings",
                typeof(CommandBindingCollection),
                typeof(RegisterCommandHelper),
                new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        #endregion

        #region

        public static CommandBindingCollection GetRegisterCommandBindings(UIElement element)
        {
            return element != null ? (CommandBindingCollection) element.GetValue(RegisterCommandBindingsProperty) : null;
        }

        private static void OnRegisterCommandBindingChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as UIElement;
            if (element == null) return;

            var bindings = e.NewValue as CommandBindingCollection;
            if (bindings != null)
            {
                // credits: http://matthamilton.net/commandbindings-with-mvvm
                // clear the collection first
                element.CommandBindings.Clear();
                element.CommandBindings.AddRange(bindings);
            }
        }

        public static void SetRegisterCommandBindings(UIElement element, CommandBindingCollection value)
        {
            if (element != null)
                element.SetValue(RegisterCommandBindingsProperty, value);
        }

        #endregion
    }
}