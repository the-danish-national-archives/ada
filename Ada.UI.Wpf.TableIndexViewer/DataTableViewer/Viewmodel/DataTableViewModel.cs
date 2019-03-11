namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Model;
    using Properties;
    using Ra.Common.Wpf.ResultsList;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;
    using ViewUtil;

    #endregion

    public class DataTableViewModel : ViewModelBase
    {
        #region  Fields

        private readonly IDbConnection _connection;

        private RelayCommand _executeQueryCommand;

        private string _queryErrorMsg;
        private int _rowFirst;
        private int _rowLast;


        private int _rowsToShow = 5;
        private RelayCommand _scrollToFirstCommand;
        private RelayCommand _scrollToLastCommand;
        private RelayCommand _scrollToNextCommand;
        private RelayCommand _scrollToPreviousCommand;
        private int _shownRows = 5;
        private int _startRow = 1;
        private DataTable _table;
        private int _totalRows;

        private string tableRequest;

        #endregion

        #region  Constructors

        [Obsolete("Only for use in designmode", true)]
        public DataTableViewModel()
            : this(new ViewModelLocator().IoC.GetInstance<TableIndex>(),
                new ViewModelLocator().IoC.GetInstance<IDbConnection>())
        {
        }

        public DataTableViewModel(TableIndex tableIndex, IDbConnection conn)
        {
            _connection = conn;
            TableIndex = tableIndex;

            QuerySelectorViewModel = new QuerySelectorViewModel(tableIndex, ExecuteQueryCommand);

            RowsToShow = Settings.Default.SimultaneousSQLItemsShownDefault;

            if (IsInDesignMode)
            {
                var table = new DataTable();
                DataColumn column;
                column = new DataColumn("Test 1");
                column.ExtendedProperties["IsPrimaryKey"] = true;
                table.Columns.Add(column);
                column = new DataColumn("Test 2");
                table.Columns.Add(column);
                column = new DataColumn("Test 3");
                column.ExtendedProperties["IsForeignKey"] = true;
                table.Columns.Add(column);


                table.Rows.Add("cell1", "cell6", "cell6");
                table.Rows.Add("cell3", "cell66", "cell66");
                Table = table;
            }
        }

        #endregion

        #region Properties

        public Func<string, List<IResultsList>> ColumnNameToContextMenu
        {
            get
            {
                return k => new List<IResultsList>
                {
                    new tempClass
                    {
                        Text = "Select distinct", Message = k,
                        Value = (Func<string[]>) (() => ExecuteForUniqueColumn(k).ToArray())
                    },
                    new tempClass {Text = ColumnNameToDecription(k), Message = k}
                };
            }
        }

        public Func<string, string> ColumnNameToDecription
        {
            get
            {
                return k => (
                    Table?.Columns.OfType<DataColumn>()
                        .FirstOrDefault(c => c.ColumnName == k)?
                        .ExtendedProperties["refColumn"] as Column)?.Description;
            }
        }

        public RelayCommand ExecuteQueryCommand
        {
            get
            {
                return _executeQueryCommand
                       ?? (_executeQueryCommand = new RelayCommand(
                           () => { ExecuteQuery(); }));
            }
        }

        public bool HasQueryError => !string.IsNullOrWhiteSpace(QueryErrorMsg);

        public string QueryErrorMsg
        {
            get => _queryErrorMsg;
            set
            {
                if (Set(ref _queryErrorMsg, value))
                    // ReSharper disable once ExplicitCallerInfoArgument
                    RaisePropertyChanged(nameof(HasQueryError), !HasQueryError, HasQueryError);
            }
        }

        public QuerySelectorViewModel QuerySelectorViewModel { get; }

        public int RowFirst
        {
            get => _rowFirst;
            set => Set(ref _rowFirst, value);
        }

        public int RowLast
        {
            get => _rowLast;
            set => Set(ref _rowLast, value);
        }

        public int RowsToShow
        {
            get => _rowsToShow;
            set => Set(ref _rowsToShow, value);
        }

        public RelayCommand ScrollToFirstCommand
        {
            get
            {
                return _scrollToFirstCommand
                       ?? (_scrollToFirstCommand = new RelayCommand(
                           () => { ExecuteQuery(0); },
                           () => RowFirst > 0));
            }
        }

        public RelayCommand ScrollToLastCommand
        {
            get
            {
                return _scrollToLastCommand
                       ?? (_scrollToLastCommand = new RelayCommand(
                           () => { ExecuteQuery(TotalRows - ShownRows); },
                           () => RowLast < TotalRows));
            }
        }

        public RelayCommand ScrollToNextCommand
        {
            get
            {
                return _scrollToNextCommand
                       ?? (_scrollToNextCommand = new RelayCommand(
                           () => { ExecuteQuery(RowLast); },
                           () => RowLast < TotalRows));
            }
        }

        public RelayCommand ScrollToPreviousCommand
        {
            get
            {
                return _scrollToPreviousCommand
                       ?? (_scrollToPreviousCommand = new RelayCommand(
                           () => { ExecuteQuery(RowFirst - ShownRows); },
                           () => RowFirst > 0));
            }
        }


        public int ShownRows
        {
            get => _shownRows;
            set => Set(ref _shownRows, value);
        }


        public int StartRow
        {
            get => _startRow;
            set => Set(ref _startRow, value);
        }


        public DataTable Table
        {
            get => _table;
            set
            {
                if (Set(ref _table, value))
                {
                    RaisePropertyChanged(nameof(ColumnNameToDecription));
                    RaisePropertyChanged(nameof(ColumnNameToContextMenu));
                }
            }
        }

        public TableIndex TableIndex { get; }


        public int TotalRows
        {
            get => _totalRows;
            set => Set(ref _totalRows, value);
        }

        #endregion

        #region

        private DataTable CreateTableFromSchema(DataTable metaTable)
        {
            var table = new DataTable();
            foreach (var metaob in metaTable.Rows)
            {
                var metaRow = metaob as DataRow;
                if (metaRow == null)
                    continue;
                var column = new DataColumn();
                foreach (DataColumn metaColumn in metaTable.Columns)
                    column.ExtendedProperties[metaColumn.ColumnName] = metaRow[metaColumn.ColumnName];

                var refColumn = GetRefColumn(column);

                column.ExtendedProperties["refColumn"] = refColumn;

                column.ExtendedProperties["IsPrimaryKey"] = refColumn?.IsPrimaryKey() ?? false;
                column.ExtendedProperties["IsForeignKey"] = refColumn?.GetForeignKeys().Any() ?? false;

                column.ColumnName = GetCoulmnName(column, refColumn);

                while (table.Columns.Contains(column.ColumnName))
                    column.ColumnName = column.ColumnName + " copy";

                table.Columns.Add(column);
            }

            return table;
        }

        private IEnumerable<string> ExecuteForUniqueColumn(string uniName)
        {
            var res = new HashSet<string>();
            if (_connection == null) return res;

            var queryString = tableRequest;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = queryString;

                command.Connection = _connection;

                try
                {
                    using (var reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        var metaTable = reader.GetSchemaTable();
                        if (metaTable == null)
                            return res;
                        var table = CreateTableFromSchema(metaTable);

                        var uniCol = table.Columns
                            .OfType<DataColumn>()
                            .FirstOrDefault(c => string.CompareOrdinal(c.ColumnName, uniName) == 0);
                        if (uniCol == null)
                            return res;


                        while (reader.Read())
                        {
                            var item = reader[(string) uniCol.ExtendedProperties["ColumnName"]];

                            res.Add(item is DBNull ? "" : (string) item);
                            if (res.Count > 100)
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return res;
        }

        private void ExecuteQuery(int? startRow = null)
        {
            if (_connection == null) return;

            var queryString = QuerySelectorViewModel.Query;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = queryString;

                command.Connection = _connection;

                try
                {
                    using (var reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        var metaTable = reader.GetSchemaTable();
                        if (metaTable == null)
                            return;
                        var table = CreateTableFromSchema(metaTable);

                        ReadQueryData(startRow ?? StartRow, table, reader);

                        Table = table;
                        tableRequest = queryString;
                    }

                    QueryErrorMsg = null;
                }
                catch (Exception e)
                {
                    QueryErrorMsg = e.Message;
                }
            }

            LatestQueries.Default.Add(
                new Query
                {
                    //                                      Name = "",
                    Value = queryString
                });
        }

        private static string GetCoulmnName(DataColumn column, Column refColumn)
        {
            var sb = new StringBuilder();
            if (refColumn != null)
                sb.Append(refColumn.ColumnId);

            if ((bool) column.ExtendedProperties["IsPrimaryKey"])
                sb.Append(" (PK)");
            if ((bool) column.ExtendedProperties["IsForeignKey"])
                sb.Append(" (FK)");
            sb.AppendLine();
            sb.Append(column.ExtendedProperties["ColumnName"]);
            if (refColumn != null &&
                string.CompareOrdinal((string) column.ExtendedProperties["ColumnName"], refColumn.Name) != 0)
                sb.Append($" ({refColumn.Name})");

            sb.AppendLine();
            if (refColumn != null)
                sb.AppendLine(refColumn.Type);


            return sb.ToString().Trim();
        }

        private Column GetRefColumn(DataColumn column)
        {
            var baseTableName = column.ExtendedProperties["BaseTableName"] as string;
            var baseColumnName = column.ExtendedProperties["BaseColumnName"] as string;
            if (baseTableName == null || baseColumnName == null)
                return null;
            return TableIndex.Tables.FirstOrDefault(
                    t => string.CompareOrdinal(t.Name.Trim('"'), baseTableName) == 0)
                ?.Columns.FirstOrDefault(
                    c =>
                        string.CompareOrdinal(c.Name.Trim('"'), baseColumnName) == 0
                );
        }

        private void ReadQueryData(int startRow, DataTable table, IDataReader reader)
        {
            ShownRows = RowsToShow;

            if (startRow < 1)
                startRow = 1;

            var emptyRows = new LinkedList<int>(Enumerable.Range(0, table.Columns.Count));

            var cRow = 0;
            if (reader.FieldCount > 0)
                while (reader.Read())
                {
                    foreach (var nonEmptyRow in emptyRows.Where(i => !string.IsNullOrEmpty(reader[i].ToString()))
                        .ToList()) emptyRows.Remove(nonEmptyRow);
                    cRow++;
                    if (cRow < startRow) continue;
                    if (cRow >= startRow + ShownRows) continue;

                    var row = table.NewRow();
                    foreach (DataColumn column in table.Columns)
                        row[column.ColumnName] = reader[(string) column.ExtendedProperties["ColumnName"]];
                    table.Rows.Add(row);
                }

            foreach (var column in table.Columns.OfType<DataColumn>().Select((d, i) => (col: d, i: i)))
                column.col.ExtendedProperties["ColumnIsEmpty"]
                    = emptyRows.Contains(column.i);

            SetShownRowNumbers(startRow, cRow);
        }

        private void SetShownRowNumbers(int startRow, int cRow)
        {
            RowFirst = startRow;
            RowLast = startRow + ShownRows - 1;
            TotalRows = cRow;

            if (RowLast > TotalRows)
                RowLast = TotalRows;

            if (RowFirst > TotalRows)
                RowLast = RowFirst;
        }

        #endregion

        #region Nested type: tempClass

        public class tempClass : IResultsList
        {
            #region IResultsList Members

            public string Text { get; set; }
            public string Message { get; set; }
            public object Value { get; set; }

            #endregion
        }

        #endregion
    }
}