namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexMissingParentColumns : AdaAvViolation
    {
        #region  Constructors

        public TableIndexMissingParentColumns(Table table, string colName)
            : base("6.C_20")
        {
            ConstraintName = table.PrimaryKey.Name;
            TableName = table.Name;
            ColumnName = colName;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ColumnName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ConstraintName { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table)
        {
            foreach (var colName in GetMissingParentColumns(table.PrimaryKey)) yield return new TableIndexMissingParentColumns(table, colName);
        }

        public static IEnumerable<string> GetMissingParentColumns(PrimaryKey pk)
        {
            //return fk.References.Select(x => x.Column).Except(fk.ParentTable.Columns.Select(x => x.Name));
            return pk.Columns.Select(x => x).Except(pk.ParentTable.Columns.Select(x => x.Name));
        }

        #endregion
    }
}