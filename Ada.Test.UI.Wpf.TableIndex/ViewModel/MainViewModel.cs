namespace Ada.UI.Wpf.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using GalaSoft.MvvmLight;
    using Ra.DomainEntities.TableIndex;
    using TableIndexViewer.ViewModel;
    using TableIndexViewer.ViewUtil;

    #endregion

    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region  Fields

        private string _dataSetName = "10001";

        private TableIndexViewerViewModel _tableIndexViewerViewModel;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            DataSetNames = new ObservableCollection<string>(new[] {"10001", "10002", "10003", "10100", "16004"});

            var tableIndex = new ViewModelLocator().IoC.GetInstance<TableIndex>();
            var dbConnection = new ViewModelLocator().IoC.GetInstance<IDbConnection>();
            TableIndexViewerViewModel = new TableIndexViewerViewModel(tableIndex, dbConnection);
        }

        #endregion

        #region Properties

        public string DataSetName
        {
            get => _dataSetName;
            set
            {
                if (Set(ref _dataSetName, value))
                {
                    var tableIndex = new ViewModelLocator().IoC.GetInstance<Func<string, TableIndex>>();
                    var dbConnection = new ViewModelLocator().IoC.GetInstance<Func<string, IDbConnection>>();
                    TableIndexViewerViewModel = new TableIndexViewerViewModel(
                        tableIndex(value),
                        dbConnection(value));
                }
            }
        }

        public ObservableCollection<string> DataSetNames { get; }

        public TableIndexViewerViewModel TableIndexViewerViewModel
        {
            get => _tableIndexViewerViewModel;
            private set => Set(ref _tableIndexViewerViewModel, value);
        }

        #endregion
    }
}