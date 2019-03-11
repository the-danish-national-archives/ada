namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class TableIndexForeignKeyColumnMissingInReferenced : AdaAvViolation
    {
        #region  Constructors

        public TableIndexForeignKeyColumnMissingInReferenced(ForeignKey foreignKey, string columnName)
            : base("6.C_21")
        {
            ConstraintName = foreignKey.Name;
            TableName = foreignKey.ParentTableName;
            ReferencedTableName = foreignKey.ReferencedTable;
            ColumnName = columnName;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ColumnName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ConstraintName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ReferencedTableName { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ForeignKey fk)
        {
            foreach (var col in GetMissingReferencedColumns(fk)) yield return new TableIndexForeignKeyColumnMissingInReferenced(fk, col);
        }

        public static IEnumerable<string> GetMissingReferencedColumns(ForeignKey fk)
        {
            IEnumerable<string> result = new List<string>();
            var refTable = fk.GetReferencedTable();
            if (refTable != null) result = fk.References.Select(x => x.Referenced).Except(refTable.Columns.Select(x => x.Name));

            return result;
        }

        #endregion
    }
}