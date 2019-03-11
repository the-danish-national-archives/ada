namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Model;
    using Properties;
    using Ra.Common.Types;
    using Ra.Common.Wpf;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;
    using Text.Properties;
    using ViewUtil;

    #endregion

    public class QuerySelectorViewModel : ViewModelBase
    {
        #region Static

        public static readonly RoutedCommand SetQueryCommand = new RoutedCommand();

        #endregion

        #region  Fields

        private readonly RelayCommand _executeQueryCommand;

        private CommandBindingCollection _commandBindings;

        private bool _isDropDownOpen;

        private string _query = "";

        private ObservableCollection<object> _selectableQueries;

        private bool _showFavorites;

        private bool _showFromAV;

        private bool _showLatest;

        #endregion

        #region  Constructors

        [Obsolete("Only for use in designmode", true)]
        public QuerySelectorViewModel()
            : this(new ViewModelLocator().IoC.GetInstance<TableIndex>(), null)
        {
        }

        public QuerySelectorViewModel(TableIndex tableIndex, RelayCommand executeQueryCommand)
        {
            _executeQueryCommand = executeQueryCommand;
            TableIndex = tableIndex;

            if (IsInDesignMode)
            {
                Query = "SELECT * FROM DOKTABEL INNER JOIN SAG ON DOKTABEL.SagsID = SAG.SagsID";
                //"SELECT * FROM DOKTABEL";

                SelectableQueries.Add(
                    new QueryViewModelView(
                        new View
                        {
                            Name = "test 1",
                            QueryOriginal = "SELECT * FROM DOKTABEL",
                            TableIndex = tableIndex
                        }));
                SelectableQueries.Add(
                    new QueryViewModelView(
                        new View
                        {
                            Name = "test 2",
                            QueryOriginal =
                                "SELECT * FROM DOKTABEL INNER JOIN\n  SAG ON DOKTABEL.SagsID = SAG.SagsID",
                            TableIndex = tableIndex
                        }));
            }

            //            foreach (var view in TableIndex.Views.Where(v => v.IsAvView()))
            //            {
            //                SelectableQueries.Add(
            //                    new QueryViewModelView(view));
            //            }


            ShowFromAV = true;
            ShowLatest = true;
            ShowFavorites = true;

            LatestQueries.Default.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(LatestQueries.Default.Queries))
                    if (ShowLatest)
                        UpdateQueryList();
            };
            FavoriteQueries.Default.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(FavoriteQueries.Default.Queries))
                    if (ShowFavorites)
                        UpdateQueryList();
            };

            UpdateQueryList();
        }

        #endregion

        #region Properties

        public CommandBindingCollection CommandBindings =>
            _commandBindings ?? (_commandBindings = new CommandBindingCollection
            {
                new CommandBinding(
                    SetQueryCommand,
                    SetQuery,
                    SetQueryCan)
            });

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set => Set(ref _isDropDownOpen, value);
        }

        public string Query
        {
            get => _query;
            set
            {
                if (value == null) value = "";
                Set(ref _query, value);
            }
        }

        public ObservableCollection<object> SelectableQueries => _selectableQueries
                                                                 ?? (_selectableQueries =
                                                                     new ObservableCollection<object>());

        public bool ShowFavorites
        {
            get => _showFavorites;
            set
            {
                Set(ref _showFavorites, value);
                UpdateQueryList();
            }
        }

        public bool ShowFromAV
        {
            get => _showFromAV;
            set
            {
                Set(ref _showFromAV, value);
                UpdateQueryList();
            }
        }

        public bool ShowLatest
        {
            get => _showLatest;
            set
            {
                Set(ref _showLatest, value);
                UpdateQueryList();
            }
        }

        public TableIndex TableIndex { get; }

        #endregion

        #region

        public event EventHandler QuerySet;

        private void RaiseQuerySet()
        {
            QuerySet?.Invoke(this, EventArgs.Empty);
        }

        private void SetQuery(object sender, ExecutedRoutedEventArgs e)
        {
            //            object o = e.Parameter;
            var matcher = new PatternMatcher<string>()
                .Case<Table>(
                    o => Queries.SelectAllFromTable
                        .Replace(Queries.QueryParameterTable, o.Name))
                .Case<Column>(
                    o => Queries.SelectOneColumnFromTable
                        .Replace(Queries.QueryParameterColumn, o.Name)
                        .Replace(Queries.QueryParameterTable, o.TableName))
                .Case<IQueryViewModel>(o => o.Query)
                .Case<Func<Func<string, PrimaryKey>, string>>(getQuery => getQuery(StringToPrimaryKey))
                .Case<Func<Func<string, ForeignKey>, string>>(getQuery => getQuery(StringToForeignKey))
                .Case<string>(o => o).Default((string) null);

            var query = matcher.Match(e.Parameter);

            if (query == null)
            {
                e.Handled = false;
                return;
            }

            Query = query;

            e.Handled = true;
            IsDropDownOpen = false;
            RaiseQuerySet();
            _executeQueryCommand?.Execute(this);
        }

        private static void SetQueryCan(object sender, CanExecuteRoutedEventArgs e)
        {
            var o = e.Parameter;
            e.CanExecute = o is Table
                           || o is Column
                           || o is string
                           || o is IQueryViewModel
                           || o is Func<Func<string, PrimaryKey>, string>
                           || o is Func<Func<string, ForeignKey>, string>;
            e.ContinueRouting = false;
        }

        public ForeignKey StringToForeignKey(string name)
        {
            return TableIndex.AllForeignKeys().FirstOrDefault(f => f.Name == name);
        }

        public PrimaryKey StringToPrimaryKey(string name)
        {
            return TableIndex.AllPrimaryKeys().FirstOrDefault(p => p.Name == name);
        }

        private void UpdateQueryList()
        {
            SelectableQueries.Clear();

            SelectableQueries.Add(CheckedViewModel.Create(this, x => x.ShowLatest, nameof(UIText.QueryLatest)));

            if (ShowLatest)
                foreach (var query in LatestQueries.Default.Queries)
                    SelectableQueries.Add(
                        new QueryViewModelView(query, TableIndex));

            SelectableQueries.Add(CheckedViewModel.Create(this, x => x.ShowFromAV, nameof(UIText.QueryFromAV)));

            if (ShowFromAV)
                foreach (var view in TableIndex.Views.Where(v => v.IsAvView()))
                    SelectableQueries.Add(
                        new QueryViewModelView(view));

            SelectableQueries.Add(CheckedViewModel.Create(this, x => x.ShowFavorites, nameof(UIText.QueryFavorites)));

            if (ShowFavorites)
                foreach (var query in FavoriteQueries.Default.Queries)
                    SelectableQueries.Add(
                        new QueryViewModelView(query, TableIndex, true));
        }

        #endregion
    }
}