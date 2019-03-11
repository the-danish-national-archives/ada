namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TablePrimaryKeyIsNullable : AdaAvViolation
    {
        #region  Constructors

        public TablePrimaryKeyIsNullable(Table table, Column nullablePkCol)
            : base("4.C.5_2")
        {
            ColumnName = nullablePkCol.Name;
            ColumnNumber = nullablePkCol.ColumnId;
            TableName = table.Name;
            FolderName = table.Folder;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ColumnName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ColumnNumber { get; set; }

        [AdaAvCheckNotificationTag]
        public string FolderName { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table)
        {
            foreach (var nullablePkCol in IsNullable(table.PrimaryKey)) yield return new TablePrimaryKeyIsNullable(table, nullablePkCol);
        }

        public static IEnumerable<Column> IsNullable(PrimaryKey pk)
        {
            var columns = pk.ParentTable.Columns.Select(x => x).Where(x => pk.Columns.Contains(x.Name));
            return columns.Where(x => x.Nullable);
        }

        #endregion
    }
}