namespace Ada.UI.Wpf.TableIndexViewer.TreeTableViewer.ViewModel
{
    #region Namespace Using

    using System;
    using System.Linq;
    using ERGraph;
    using GalaSoft.MvvmLight;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;
    using ViewUtil;

    #endregion

    public class TableFilterViewModel : ViewModelBase
    {
        #region FunctionalDescriptionEnum enum

        public enum FunctionalDescriptionEnum
        {
            Myndighedsidentifikation = 1,
            Dokumentidentifikation = 2,
            Lagringsform = 3,
            Afleveret = 4,
            Sagsidentifikation = 5,
            Sagstitel = 6,
            Dokumenttitel = 7,
            Dokumentdato = 8,
            Afsender_modtager = 9,
            Digital_signatur = 10,
            FORM = 11,
            Kassation = 12
        }

        #endregion

        #region SelectedRelationEnum enum

        public enum SelectedRelationEnum
        {
            Any,
            Neighbors,
            NeighborsIn,
            NeighborsOut,
            Only
        }

        #endregion

        #region  Fields

        private Func<Table, bool> _filter;

        private bool _functionalDescriptionEnabled;

        private FunctionalDescriptionEnum _selectedFunctionalDescription = FunctionalDescriptionEnum.Sagsidentifikation;

        private SelectedRelationEnum _selectedRelation = SelectedRelationEnum.Any;


        private bool _selectedRelationEnabled;

        private string _textSearch = "";

        #endregion

        #region  Constructors

//        public readonly Func<Table, bool> DefaultFilter = t => true;


        [Obsolete("Only for use in designmode", true)]
        public TableFilterViewModel()
            : this(new ViewModelLocator().IoC.GetInstance<TableIndex>(), null)
        {
        }

        public TableFilterViewModel(TableIndex tableIndex, ISelectionDetector selectionDetector)
        {
            TableIndex = tableIndex;
            SelectionDetector = selectionDetector;
            selectionDetector.SelectionChanged += () => UpdateFilter();
        }

        #endregion

        #region Properties

        public Func<Column, bool> ColumnMarked { get; private set; }

        public Func<Table, bool> Filter
        {
            get => _filter;

            private set => Set(ref _filter, value);
        }

        public bool FunctionalDescriptionEnabled
        {
            get => _functionalDescriptionEnabled;

            set
            {
                if (Set(ref _functionalDescriptionEnabled, value))
                    UpdateFilter();
            }
        }

        public FunctionalDescriptionEnum SelectedFunctionalDescription
        {
            get => _selectedFunctionalDescription;

            set
            {
                if (Set(ref _selectedFunctionalDescription, value))
                    UpdateFilter();
            }
        }

        public SelectedRelationEnum SelectedRelation
        {
            get => _selectedRelation;
            set
            {
                if (Set(ref _selectedRelation, value))
                    UpdateFilter();
            }
        }

        public bool SelectedRelationEnabled
        {
            get => _selectedRelationEnabled;

            set
            {
                if (Set(ref _selectedRelationEnabled, value))
                    UpdateFilter();
            }
        }

        public ISelectionDetector SelectionDetector
        {
            get;

//            set
//            {
//                _selectionDetector = value;
//            }
        }

        public TableIndex TableIndex { get; }

        public Func<Table, bool> TableMarked { get; private set; }

        public string TextSearch
        {
            get => _textSearch;

            set
            {
                if (Set(ref _textSearch, value))
                    UpdateFilter();
            }
        }

        #endregion

        #region

        public bool ShouldMark(object data)
        {
            switch (data)
            {
                case Table t:
                    return TableMarked?.Invoke(t) ?? false;
                case Column c:
                    return ColumnMarked?.Invoke(c) ?? false;
            }

            return false;
        }


        private void UpdateFilter()
        {
            void CaptureAndAdd<T>(ref Func<T, bool> toCapture, Func<T, bool> toAdd)
            {
                var captured = toCapture ?? (_ => true);
                toCapture = p => captured(p) && toAdd(p);
            }

            Func<Table, bool> filter = null;
            Func<Table, bool> tableFilter = null;
            Func<Column, bool> columnFilter = null;

            if (FunctionalDescriptionEnabled)
            {
                var functionalDescription =
                    (FunctionalDescription)
                    Enum.Parse(typeof(FunctionalDescription), SelectedFunctionalDescription.ToString());

                Func<Column, bool> cf = c => c.FunctionalDescriptions.Any(f => f == functionalDescription);

                CaptureAndAdd(ref columnFilter, cf);

                // do not mark table, to fokus on column
                CaptureAndAdd(ref tableFilter, t => false);


                CaptureAndAdd(ref filter, t => t.Columns.Any(cf));
            }

            if (!string.IsNullOrWhiteSpace(TextSearch))
            {
                var textSearch = TextSearch;
                var filterBefore = filter;
                Func<string, bool> comp = s => s.IndexOf(textSearch, StringComparison.InvariantCultureIgnoreCase) >= 0;

                Func<Column, bool> cf = c => comp(c.Name);
                Func<Table, bool> tf = t => comp(t.Name);

                CaptureAndAdd(ref columnFilter, cf);

                CaptureAndAdd(ref tableFilter, tf);

                CaptureAndAdd(ref filter, t => tf(t) || t.Columns.Any(cf));
            }


            if (SelectedRelation != SelectedRelationEnum.Any)
            {
                var selectedRelation = SelectedRelation;
//                ti.AllForeignKeys().Where(f => f.
                Func<Table, bool> selectedFilter = t => SelectionDetector?.IsSelected(t) ?? false;
                Func<Table, bool> neighborsInFilter = t => t.ForeignKeys.Select(f => f.ReferencedTable).Any(rt => SelectionDetector?.IsSelected(rt) ?? false);
                Func<Table, bool> neighborsOutFilter =
                    t => TableIndex.AllForeignKeys().Any(f => f.ReferencedTable == t.Name && (SelectionDetector?.IsSelected(f.ParentTableName) ?? false));

                Func<Table, bool> relationFilter = null;

                switch (selectedRelation)
                {
                    case SelectedRelationEnum.Any:
                        break;
                    case SelectedRelationEnum.Neighbors:
                        relationFilter = t => !selectedFilter(t) && (neighborsInFilter(t) || neighborsOutFilter(t));
                        break;
                    case SelectedRelationEnum.NeighborsIn:
                        relationFilter = t => !selectedFilter(t) && neighborsInFilter(t);
                        break;
                    case SelectedRelationEnum.NeighborsOut:
                        relationFilter = t => !selectedFilter(t) && neighborsOutFilter(t);
                        break;
                    case SelectedRelationEnum.Only:
                        relationFilter = t => selectedFilter(t);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (relationFilter != null)
                {
                    var tf = relationFilter;


                    CaptureAndAdd(ref tableFilter, tf);

                    CaptureAndAdd(ref filter, tf);
                }
            }

            TableMarked = tableFilter;
            ColumnMarked = columnFilter;

            Filter = filter;
        }

        #endregion
    }
}