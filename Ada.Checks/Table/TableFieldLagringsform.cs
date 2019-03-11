namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableFieldLagringsform : AdaAvViolation
    {
        #region  Constructors

        private TableFieldLagringsform(Table table, string column, int count)
            : base("6.C_16")
        {
            Table = table.Name;
            Column = column;
            Count = count;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Column { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTag]
        public string Table { get; set; }

        #endregion

        #region Nested type: TableFieldLagringsformSummery

        public class TableFieldLagringsformSummery
        {
            #region  Fields

            private readonly List<string> columnOfLagringsform;

            private readonly Dictionary<string, int> columnToErrorCount = new Dictionary<string, int>();
            private readonly HashSet<string> okValues;

            #endregion

            #region  Constructors

            public TableFieldLagringsformSummery(Table table)
            {
                Table = table;

                columnOfLagringsform =
                    Table.Columns.Where(col => col.FunctionalDescriptions.Contains(FunctionalDescription.Lagringsform))
                        .Select(col => col.ColumnId)
                        .ToList();

                okValues = new HashSet<string>
                {
                    "1",
                    "2",
                    "3"
                };
            }

            #endregion

            #region Properties

            private Table Table { get; }

            #endregion

            #region

            public void Check(SortedList<string, string> rowContents)
            {
                foreach (var colId in columnOfLagringsform)
                {
                    string content;
                    rowContents.TryGetValue(colId, out content);

                    if (rowContents.TryGetValue(colId, out content)
                        && !okValues.Contains(content))
                    {
                        var countBefore = columnToErrorCount.GetOrDefault(colId);

                        columnToErrorCount[colId] = countBefore + 1;
                    }
                }
            }

            public IEnumerable<AdaAvCheckNotification> Report()
            {
                foreach (var kvp in columnToErrorCount) yield return new TableFieldLagringsform(Table, kvp.Key, kvp.Value);
            }

            #endregion
        }

        #endregion
    }
}