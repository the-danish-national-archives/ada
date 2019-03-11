namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    [AdaAvCheckToResultsList(nameof(Rows), ',')]
    public class TableLeadingOrTrailingWhitespace : AdaAvViolation
    {
        #region  Constructors

        public TableLeadingOrTrailingWhitespace(string table, string column, int count, IList<long> rows)
            : base("4.D_12")
        {
            Table = table;
            Column = column;
            Count = count;
            Rows = rows.Select(r => r.ToString()).ToList();
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Column { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTagSmartToString(Seperator = ",", Hidden = true)]
        public IEnumerable<string> Rows { get; set; }

        [AdaAvCheckNotificationTag]
        public string Table { get; set; }

        #endregion

        #region Nested type: TableLeadingOrTrailingWhitespaceSummery

        public class TableLeadingOrTrailingWhitespaceSummery
        {
            #region  Fields

            private readonly Dictionary<string, (int count, IList<long> rows)> columnToErrorCount = new Dictionary<string, (int count, IList<long> rows)>();

            #endregion

            #region  Constructors

            public TableLeadingOrTrailingWhitespaceSummery(Table table)
            {
                Table = table.Name;
            }

            #endregion

            #region Properties

            private string Table { get; }

            #endregion

            #region

            public void Check(SortedList<string, string> rowContents, long rowId)
            {
                foreach (var kvp in rowContents)
                {
                    if (string.IsNullOrEmpty(kvp.Value))
                        continue;
                    if (!char.IsWhiteSpace(kvp.Value[0]) && !char.IsWhiteSpace(kvp.Value[kvp.Value.Length - 1]))
                        continue;

                    var v = columnToErrorCount.GetOrDefault(kvp.Key);

                    const int rowsToCollect = 1000;

                    if (v.rows == null)
                        v.rows = new List<long>(rowsToCollect);

                    if (v.rows.Count < rowsToCollect)
                        v.rows.Add(rowId);

                    columnToErrorCount[kvp.Key] = (v.count + 1, v.rows);
                }
            }

            public IEnumerable<AdaAvCheckNotification> Report()
            {
                foreach (var kvp in columnToErrorCount) yield return new TableLeadingOrTrailingWhitespace(Table, kvp.Key, kvp.Value.count, kvp.Value.rows);
            }

            #endregion
        }

        #endregion
    }
}