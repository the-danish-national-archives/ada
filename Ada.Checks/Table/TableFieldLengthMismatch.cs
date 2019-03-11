namespace Ada.Checks.Table
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableFieldLengthMismatch : AdaAvViolation
    {
        #region  Constructors

        private TableFieldLengthMismatch(Table table, string column, int count, int max)
            : base("6.C_25")
        {
            TableName = table.Name;
            ColumnName = column;
            Count = count;
            ColumnLength = max;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int ColumnLength { get; set; }

        [AdaAvCheckNotificationTag]
        public string ColumnName { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region Nested type: TableFieldLengthMismatchSummery

        public class TableFieldLengthMismatchSummery
        {
            #region  Fields

            private readonly Dictionary<string, (int count, int max)> columnToErrorCount =
                new Dictionary<string, (int, int)>();

            private readonly Dictionary<string, int> columnToLengths = new Dictionary<string, int>();

            private readonly Dictionary<string, int> columnToMaxLength = new Dictionary<string, int>();

            #endregion

            #region  Constructors

            public TableFieldLengthMismatchSummery(Table table)
            {
                Table = table;


                foreach (var fieldType in Table.Columns.Select(col => new {col.ColumnId, col.Type}).ToList())
                {
                    int length;
                    var split = fieldType.Type.Split('(', ')');
                    if (split.Length > 1)
                        if (int.TryParse(split[1], out length))
                            columnToLengths.Add(fieldType.ColumnId, length);
                }
            }

            #endregion

            #region Properties

            private Table Table { get; }

            #endregion

            #region

            public void Check(SortedList<string, string> rowContents)
            {
                foreach (var kvp in columnToLengths)
                {
                    rowContents.TryGetValue(kvp.Key, out var content);
                    if (content != null)
                    {
                        var contentLength = content.Replace("''", "'").Length;
                        if (contentLength > kvp.Value)
                        {
                            var (count, max) = columnToErrorCount.GetOrDefault(kvp.Key);
                            columnToErrorCount[kvp.Key] = (count + 1, Math.Max(max, contentLength));
                        }
                    }
                }
            }

            public IEnumerable<AdaAvCheckNotification> Report()
            {
                foreach (var kvp in columnToErrorCount)
                    yield return new TableFieldLengthMismatch(Table, kvp.Key, kvp.Value.count, kvp.Value.max);
            }

            #endregion
        }

        #endregion
    }
}