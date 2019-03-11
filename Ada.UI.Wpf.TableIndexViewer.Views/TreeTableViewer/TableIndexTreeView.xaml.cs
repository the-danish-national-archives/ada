namespace Ada.UI.Wpf.TableIndexViewer.Views.TreeTableViewer
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Ra.DomainEntities.TableIndex;

    #endregion

    /// <summary>
    ///     Interaction logic for TreeTableViewer.xaml
    /// </summary>
    public partial class TableIndexTreeView : UserControl
    {
        #region  Constructors

        public TableIndexTreeView()
        {
            InitializeComponent();
        }

        #endregion

        #region

        private void TablesTreeItem_OnMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var context = (sender as FrameworkElement)?.DataContext;

                // Package the data.
                var data = new DataObject();

                if (context is Table)
                {
                    data.SetData(context.GetType(), context);
                    data.SetText(((Table) context).Name);
                }
                else if (context is Column)
                {
                    data.SetData(context.GetType(), context);
                    data.SetText(((Column) context).Name);
                }

                if (data.ContainsText()) DragDrop.DoDragDrop(sender as DependencyObject ?? this, data, DragDropEffects.Move);
            }
        }

        #endregion
    }
}