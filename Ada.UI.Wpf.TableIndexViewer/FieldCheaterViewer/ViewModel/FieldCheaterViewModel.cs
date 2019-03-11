namespace Ada.UI.Wpf.TableIndexViewer.FieldCheaterViewer.ViewModel
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ExtensionMethods;
    using GalaSoft.MvvmLight;
    using NHibernate.Util;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;
    using Text.Properties;

    #endregion

    public class FieldCheaterViewModel : ViewModelBase
    {
        #region  Fields

        private readonly object _baseItem;
        private IEnumerable<KeyValuePair<string, string>> _list = Enumerable.Empty<KeyValuePair<string, string>>();

        #endregion

        #region  Constructors

        public FieldCheaterViewModel(object baseItem = null)
        {
            if (baseItem is FieldCheaterViewModel prev)
            {
                _list = prev._list;
                _baseItem = prev._baseItem;
                return;
            }

            _baseItem = baseItem;
            var dic = new List<KeyValuePair<string, string>>();
            switch (baseItem)
            {
                case Table table:
                    dic.Add(nameof(UIText.Tablename), table.Name);
                    dic.Add(nameof(UIText.Folder), table.Folder);
                    dic.Add(nameof(UIText.Rows), table.Rows);
                    dic.Add(nameof(UIText.Description), table.Description);
                    List = dic;
                    Title = $"{table.Name}";
                    return;
                case Column column:
                    dic.Add(nameof(UIText.Tablename), column.ParentTable.Name);
                    dic.Add(nameof(UIText.Folder), column.ParentTable.Folder);
                    dic.Add(nameof(UIText.ColumnName), column.Name);
                    dic.Add(nameof(UIText.ColumnId), column.ColumnId);
                    dic.Add(nameof(UIText.Type), column.Type);
                    dic.Add(nameof(UIText.TypeOriginal), column.TypeOriginal);
                    dic.Add(nameof(UIText.DefaultValue), column.DefaultValue);
                    dic.Add(nameof(UIText.Nullable), column.Nullable ? UIText.True : UIText.False);
                    dic.Add(nameof(UIText.IsPrimaryKey), column.IsPrimaryKey() ? UIText.Yes : UIText.No);
                    if (column.IsPrimaryKey())
                        dic.Add(nameof(UIText.PrimaryKeyName), column.ParentTable.PrimaryKey.ConstraintColumns
                                                                   .FirstOrDefault(cc => string.CompareOrdinal(column.Name, cc.Column) == 0)?.ConstraintName ?? "");

                    dic.Add(nameof(UIText.Description), column.Description);
                    column.FunctionalDescriptions.Select(
                            (f, i) => new {text = i == 0 ? nameof(UIText.FunctionalDescriptions) : "", value = f.ToString()})
                        .ForEach(a => dic.Add(a.text, a.value));
                    List = dic;
                    Title = $"{column.Name}";
                    return;
                case Reference refe:
                    dic.Add(nameof(UIText.ColumnName), refe.Column);
                    dic.Add(nameof(UIText.referenced), refe.Referenced);
                    dic.Add(nameof(UIText.referencedTable), refe.ParentConstraint.ReferencedTable);
                    dic.Add(nameof(UIText.ForeignKeyName), refe.ParentConstraint.Name);
                    List = dic;
                    Title = $"{refe.Column}";
                    return;
                default:
                    List = Enumerable.Empty<KeyValuePair<string, string>>();
                    return;
            }
        }

        #endregion

        #region Properties

        public IEnumerable<KeyValuePair<string, string>> List
        {
            get => _list;

            set => Set(ref _list, value);
        }

        public string Title { get; }

        #endregion

        #region

        public override string ToString()
        {
            return $"{GetType().Name}: {List.Count()}";
        }

        #endregion
    }
}