namespace Ra.Common.Wpf.ResultsList
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using Types;

    #endregion

    public class ResultsListViewModel : ViewModelBase, IDisposable
    {
        #region Static

        public static readonly RoutedCommand SetQueryCommand = new RoutedCommand();

        #endregion

        #region  Fields

        private CommandBindingCollection _commandBindings;


        private string _query = "";

        private string _queryErrorMsg;

        private DataTable _table;

        private string message;
        private bool noHeader;

        #endregion

        #region  Constructors

        public ResultsListViewModel(IDbConnection testConn)
        {
            Connection = testConn;
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

        private IDbConnection Connection { get; set; }

        public bool HasQueryError => !string.IsNullOrWhiteSpace(QueryErrorMsg);

        public string Message
        {
            get => message;
            set => Set(ref message, value);
        }

        public bool NoHeader
        {
            get => noHeader;
            set => Set(ref noHeader, value);
        }

        public string Query
        {
            get => _query;
            set
            {
                if (value == null) value = "";
                if (Set(ref _query, value)) UpdateDataTable();
            }
        }

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

        public DataTable Table
        {
            get => _table;
            set => Set(ref _table, value);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Connection = null;
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
                foreach (DataColumn metaColumn in metaTable.Columns) column.ExtendedProperties[metaColumn.ColumnName] = metaRow[metaColumn.ColumnName];

                column.ColumnName = metaRow["ColumnName"] as string;

                while (table.Columns.Contains(column.ColumnName ?? ""))
                    column.ColumnName = column.ColumnName + " copy";

                table.Columns.Add(column);
            }

            return table;
        }


        private void FillTable(string[] values)
        {
            var table = new DataTable();
            var column = table.Columns.Add("1");
            foreach (var value in values) table.Rows.Add(value);
            Table = table;
            NoHeader = true;
        }

        private void ReadQueryData(int startRow, DataTable table, IDataReader reader)
        {
            if (startRow < 1)
                startRow = 1;

            var cRow = 0;
            if (reader.FieldCount > 0)
                while (reader.Read())
                {
                    cRow++;
//                    if (cRow < startRow) continue;
//                    if (cRow >= startRow + ShownRows) continue;

                    var row = table.NewRow();
                    foreach (DataColumn column in table.Columns) row[column.ColumnName] = reader[(string) column.ExtendedProperties["ColumnName"]];
                    table.Rows.Add(row);
                }
        }

        private void SetQuery(object sender, ExecutedRoutedEventArgs e)
        {
            Message = (e.Parameter as IResultsList)?.Message;
            var parameterValue = (e.Parameter as IResultsList)?.Value ?? e.Parameter;

            var valuelistFunc = parameterValue as Func<string[]>;
            if (valuelistFunc != null)
            {
                FillTable(valuelistFunc());
                return;
            }

            //            object o = parameterValue;
            var matcher = new PatternMatcher<string>()
                .Case<string>(o => o).Default((string) null);

            var query = matcher.Match(parameterValue);

            if (query == null)
            {
                e.Handled = false;
                return;
            }

            var oldTable = Table;

            Query = query;

            if (ReferenceEquals(oldTable, Table))
                UpdateDataTable();

            e.Handled = true;
            //            IsDropDownOpen = false;
            //            RaiseQuerySet();
            //            _executeQueryCommand?.Execute(this);
        }

        private static void SetQueryCan(object sender, CanExecuteRoutedEventArgs e)
        {
            var o = e.Parameter;
            e.CanExecute = o is string
                           || o is Func<string[]>
                           || (o as IResultsList)?.Value is string
                           || (o as IResultsList)?.Value is Func<string[]>;
            e.ContinueRouting = false;
        }

        public override string ToString()
        {
            return Query;
        }

        private void UpdateDataTable()
        {
            if (Connection == null) return;

            var queryString = Query;
            if (Query == "")
            {
                Table = null;
                NoHeader = true;
            }

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = queryString;

                command.Connection = Connection;

                try
                {
                    using (var reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        var metaTable = reader.GetSchemaTable();
                        if (metaTable == null)
                            return;
                        var table = CreateTableFromSchema(metaTable);

                        ReadQueryData(0, table, reader);

                        Table = table;
                        NoHeader = false;
                    }

                    QueryErrorMsg = null;
                }
                catch (Exception e)
                {
                    QueryErrorMsg = e.Message;
                }
            }
        }

        #endregion

//        public IValueConverter Converter
    }
}