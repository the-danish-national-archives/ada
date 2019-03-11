namespace Ra.Common.Wpf.Utils.Interactivity
{
    #region Namespace Using

    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    #endregion

    public class HideNotCloseBehavior : Behavior<Window>
    {
        #region

        private void AssociatedWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            AssociatedObject.Visibility = Visibility.Hidden;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += AssociatedWindow_Closing;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Closing -= AssociatedWindow_Closing;
            base.OnDetaching();
        }

        #endregion
    }
}