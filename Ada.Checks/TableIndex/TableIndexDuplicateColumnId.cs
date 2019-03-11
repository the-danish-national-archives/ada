namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexDuplicateColumnId : AdaAvViolation
    {
        #region  Constructors

        public TableIndexDuplicateColumnId(Table table, string name, int count)
            : base("6.C_3")
        {
            TableName = table.Name;
            ColumnID = name;
            Count = count;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ColumnID { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table)
        {
            foreach (var duplicate in
                table.Columns.Select(x => x.ColumnId).GroupBy(x => x).Where(x => x.Count() > 1).ToDictionary(x => x.Key, y => y.Count())
            )
                yield return new TableIndexDuplicateColumnId(table, duplicate.Key, duplicate.Value);
        }

        #endregion
    }
}