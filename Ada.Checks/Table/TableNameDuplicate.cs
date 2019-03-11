namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableNameDuplicate : AdaAvViolation
    {
        #region  Constructors

        public TableNameDuplicate(string tableName, int count)
            : base("4.C.5_3")
        {
            TableName = tableName;
            Count = count;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(IList<Table> tables)
        {
            foreach (var tableNameDuplicate in tables.GroupBy(x => x.Name)
                .Where(g => g.Count() > 1)
                .Select(y => new {Name = y.Key, Count = y.Count()})
            )
                yield return new TableNameDuplicate(tableNameDuplicate.Name, tableNameDuplicate.Count);
        }

        #endregion
    }
}