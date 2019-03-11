namespace Ada.UI.Wpf.TableIndexViewer.TreeTableViewer.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using ERGraph;
    using FieldCheaterViewer.ViewModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TreeTableViewModel : ViewModelBase
    {
        #region  Fields

        private ICommand _collapseCommand;

        private ICommand _expandCommand;

        private FieldCheaterViewModel _fieldCheater = new FieldCheaterViewModel(null);

        private IEnumerable<Table> _listedTables;

        private object _selectedField;

        private RelayCommand<object> _setSelectedCommand;

        private RelayCommand<object> _setSelectedTabelCommand;

        #endregion

        #region  Constructors

        public TreeTableViewModel(TableIndex tableIndex, ISelectionDetector selectionDetector)
        {
            TableIndex = tableIndex;
            PropertyChanged += OnPropertyChanged;

            ListedTables = tableIndex.Tables;
            SetSelectedCommand.Execute(tableIndex.Tables.FirstOrDefault());
            TableFilterViewModel = new TableFilterViewModel(tableIndex, selectionDetector);
            TableFilterViewModel.PropertyChanged += TableFilterViewModel_PropertyChanged;
        }

        #endregion

        #region Properties

        public ICommand CollapseCommand
        {
            get => _collapseCommand;
            set => Set(ref _collapseCommand, value);
        }

        public ICommand ExpandCommand
        {
            get => _expandCommand;
            set => Set(ref _expandCommand, value);
        }

        public FieldCheaterViewModel FieldCheater
        {
            get => _fieldCheater;

            set => Set(ref _fieldCheater, value);
        }

        public IEnumerable<Table> ListedTables
        {
            get => _listedTables;
            set => Set(ref _listedTables, value);
        }

        public object SelectedField
        {
            get => _selectedField;
            set
            {
                if (value == null || value is Table || value is Column || value is Reference)
                    Set(ref _selectedField, value);
                else
                    throw new ArgumentException($"Bad type, saw {value.GetType().Name}", nameof(SelectedField));
            }
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

        public RelayCommand<object> SetSelectedTabelCommand
        {
            get
            {
                return _setSelectedTabelCommand
                       ?? (_setSelectedTabelCommand = new RelayCommand<object>(
                           p => { SelectedField = TableIndex.Tables.FirstOrDefault(t => string.CompareOrdinal(t.Name, p as string) == 0); },
                           p => p as string != null));
            }
        }

        public TableFilterViewModel TableFilterViewModel { get; }
        public TableIndex TableIndex { get; }

        #endregion

        #region

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs eArg)
        {
            switch (eArg.PropertyName)
            {
                case nameof(SelectedField):
                    UpdateFieldProperites();
                    break;
            }
        }

        private void TableFilterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TableFilterViewModel.Filter):

                    Debug.WriteLine($"{DateTime.Now:O} Tables.Where");
                    ListedTables = TableIndex.Tables.Where(TableFilterViewModel.Filter ?? (_ => true));
                    Debug.WriteLine($"{DateTime.Now:O} §§Tables.Where");
                    break;
            }
        }

        private void UpdateFieldProperites()
        {
            FieldCheater = new FieldCheaterViewModel(SelectedField);
        }

        #endregion
    }
}