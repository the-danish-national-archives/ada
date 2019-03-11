#region Header

// Author 
// Created 25

#endregion

namespace Ada.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Log;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Wpf.ResultsList;

    #endregion

    public class ProgressManagementViewModel : ViewModelBase, IDisposable
    {
        #region Static

        public static readonly RoutedCommand SetSelectedCommandRouted = new RoutedCommand();

        #endregion

        #region  Fields

        private CommandBindingCollection _commandBindings;

        private ResultsListViewModel _resultsListViewModel;


        private object _selectedField;

        private RelayCommand<object> _setSelectedCommand;

        private ICommand runTestCommand;

        private AdaTestLog testLog;

        #endregion

        #region  Constructors

        public ProgressManagementViewModel(IDbConnection testConnection = null)
        {
            TestConnection = testConnection;
            var myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/Ada.UI.Wpf.ProgressManagement;component/ModelData/CheckTreeItems.xaml",
                    UriKind.RelativeOrAbsolute);
            CheckTree = ((CheckTreeItem) myResourceDictionary["Here"]).YieldOrEmpty(); //.SubItems;

            SelectedField = CheckTree.FirstOrDefault();

            CommandBindings.AddRange(ResultsListViewModel.CommandBindings);
        }

        #endregion

        #region Properties

        public IEnumerable<CheckTreeItem> CheckTree { get; }

        public CommandBindingCollection CommandBindings =>
            _commandBindings ?? (_commandBindings = new CommandBindingCollection
            {
                new CommandBinding(
                    SetSelectedCommandRouted,
                    SetSelectedRouted
//                                                                SetQueryCan
                )
            });

        public ResultsListViewModel ResultsListViewModel => _resultsListViewModel ?? (_resultsListViewModel = new ResultsListViewModel(TestConnection));

        public ICommand RunTestCommand => runTestCommand ?? (runTestCommand = new RelayCommand(OnRunTestRequested));

        public object SelectedField
        {
            get => _selectedField;
            set => Set(ref _selectedField, value);
        }

        /// <summary>
        ///     Gets the SetSelectedCommand.
        /// </summary>
        public RelayCommand<object> SetSelectedCommand
        {
            get
            {
                return _setSelectedCommand
                       ?? (_setSelectedCommand = new RelayCommand<object>(
                           p => { SelectedField = p; }));
            }
        }

        private IDbConnection TestConnection { get; set; }

        public AdaTestLog TestLog
        {
            get => testLog;

            set
            {
                if (Set(ref testLog, value))
                    foreach (var item in CheckTree)
                        item.TestLog = testLog;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _resultsListViewModel?.Dispose();
            TestConnection = null;
        }

        #endregion

        #region

        protected virtual void OnRunTestRequested()
        {
            RunTestRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler RunTestRequested;


        private void SetSelectedRouted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
                SelectedField = e.Parameter;
        }

        public override string ToString()
        {
            return "ProgressViewModel";
        }

        #endregion

//        public ProgressManagementViewModel()//Controller controller)
//        {
//            var checkTree = new List<CheckTreeItem>();
//            
//            foreach (var order in Controller.ExecutionOrders)
//            {
//                checkTree.Add(new CheckTreeItem(order));
//            }
//            _checkTree = checkTree;
//        }
    }
}