namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexDuplicateColumnName : AdaAvViolation
    {
        #region  Constructors

        public TableIndexDuplicateColumnName(Table table, string columnName, int count)
            : base("6.C_2")
        {
            TableName = table.Name;
            ColumnName = columnName;
            Count = count;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ColumnName { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table)
        {
            foreach (var duplicate in
                table.Columns.Select(x => x.Name)
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .ToDictionary(x => x.Key, y => y.Count()))
                yield return new TableIndexDuplicateColumnName(table, duplicate.Key, duplicate.Value);
//            "SELECT colName as ColumnName, columns.tableName AS TableName, folder as FolderName, COUNT(colName) as Count FROM columns 
//
//                                                INNER JOIN tables ON tables.tableName = columns.tableName
//
//                                                GROUP BY colName, columns.tableName HAVING count > 1" 
        }

        #endregion
    }
}