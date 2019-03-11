namespace Ada.UI.Wpf.TableIndexViewer.Views.DataTableViewer
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Controls;

    #endregion

    /// <summary>
    ///     Interaction logic for QuerySelectorView.xaml
    /// </summary>
    public partial class QuerySelectorView : UserControl
    {
        #region  Constructors

        public QuerySelectorView()
        {
            InitializeComponent();
        }

        #endregion

        #region

        private void Popup_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Popup_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion
    }
}