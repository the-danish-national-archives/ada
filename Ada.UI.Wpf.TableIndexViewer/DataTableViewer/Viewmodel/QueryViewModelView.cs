namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.ViewModel
{
    #region Namespace Using

    using System;
    using GalaSoft.MvvmLight;
    using Model;
    using Ra.DomainEntities.TableIndex;
    using ViewUtil;

    #endregion

    public class QueryViewModelView : ViewModelBase, IQueryViewModel
    {
        #region  Fields

        private readonly View _view;


        private bool _isFavorite;

        #endregion

        #region  Constructors

        [Obsolete("Only for use in designmode", true)]
        public QueryViewModelView()
            : this(new View
            {
                TableIndex = new ViewModelLocator().IoC.GetInstance<TableIndex>(),
                Name = "AV_Dokumenter_til_sag",
                QueryOriginal =
                    "SELECT d.Dokumenttitel, d.Dato, d.DokumentID FROM sag s, DOKTABEL d WHERE s.SagsID = d.SagsID and s.sagsid = 2",
                Description = "SQL forespørgsel, der finder alle dokumenter til en bestemt sag"
            })
        {
        }

        public QueryViewModelView(View view, bool isFavorite = false)
        {
            _view = view;
            _isFavorite = isFavorite;
        }

        public QueryViewModelView(Query query, TableIndex tableIndex, bool isFavorite = false)
            : this(
                new View
                {
                    Description = "",
//                    Name = query.Name,
                    QueryOriginal = query.Value, TableIndex = tableIndex
                },
                isFavorite)
        {
        }

        #endregion

        #region IQueryViewModel Members

        public string Query => _view.QueryOriginal;

        public string Name => _view.Name;

        public bool HasName => !string.IsNullOrWhiteSpace(Name);

        public string Description => _view.Description;

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite == value) return;

                if (value)
                {
                    var temp = new Query
                    {
//                        Name = _view.Name,
                        Value = _view.QueryOriginal
                    };
                    LatestQueries.Default.Remove(temp);
                    FavoriteQueries.Default.Add(temp);
                }
                else
                {
                    var temp = new Query
                    {
//                                       Name = _view.Name,
                        Value = _view.QueryOriginal
                    };
                    FavoriteQueries.Default.Remove(temp);
                    LatestQueries.Default.Add(temp);
                }

                _isFavorite = value;
            }
        }

        public TableIndex TableIndex => _view.TableIndex;

        #endregion
    }
}