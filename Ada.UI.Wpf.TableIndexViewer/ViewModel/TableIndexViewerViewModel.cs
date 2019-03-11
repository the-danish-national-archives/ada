namespace Ada.UI.Wpf.TableIndexViewer.ViewModel
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Windows.Input;
    using DataTableViewer.ViewModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using GraphTableViewer.ViewModel;
    using Ra.DomainEntities.TableIndex;
    using TreeTableViewer.ViewModel;
    using ViewUtil;

    #endregion

    public class TableIndexViewerViewModel : ViewModelBase
    {
        #region  Fields

        private CommandBindingCollection _commandBindings;

        private TableIndex _tableIndex;

        #endregion

        #region  Constructors

        [Obsolete("Only for use in designmode", true)]
        public TableIndexViewerViewModel()
            : this(new ViewModelLocator().IoC.GetInstance<TableIndex>(), new ViewModelLocator().IoC.GetInstance<IDbConnection>())
        {
            Source = "Simple";
        }

        [PreferredConstructor]
        public TableIndexViewerViewModel(TableIndex tableIndex, IDbConnection conn)
        {
            if (tableIndex == null)
                throw new ArgumentNullException(nameof(tableIndex));


            TableIndex = tableIndex;
            Source = "Complex";
            SqlShowViewModel = new DataTableViewModel(tableIndex, conn);
            GraphTableViewModel = new GraphTableViewModel(tableIndex);
            TreeTableViewModel = new TreeTableViewModel(tableIndex, GraphTableViewModel.SelectionDetector);
        }

        #endregion

        #region Properties

        public CommandBindingCollection CommandBindings
            =>
                _commandBindings
                ?? (_commandBindings =
                    new CommandBindingCollection(SqlShowViewModel.QuerySelectorViewModel.CommandBindings));

        public GraphTableViewModel GraphTableViewModel { get; }

        public string Source { get; }

        public DataTableViewModel SqlShowViewModel { get; }

        public TableIndex TableIndex
        {
            get => _tableIndex;
            set => Set(ref _tableIndex, value);
        }

        public TreeTableViewModel TreeTableViewModel { get; }

        #endregion
    }
}