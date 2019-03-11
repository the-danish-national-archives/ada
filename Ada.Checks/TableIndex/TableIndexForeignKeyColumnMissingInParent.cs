namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexForeignKeyColumnMissingInParent : AdaAvViolation
    {
        #region  Constructors

        public TableIndexForeignKeyColumnMissingInParent(ForeignKey foreignKey, string columnName)
            : base("6.C_22")
        {
            ConstraintName = foreignKey.Name;
            TableName = foreignKey.ParentTableName;
            ColumnName = columnName;
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

        public static IEnumerable<AdaAvCheckNotification> Check(ForeignKey fk)
        {
            foreach (var col in GetMissingParentColumns(fk)) yield return new TableIndexForeignKeyColumnMissingInParent(fk, col);
        }

        public static IEnumerable<string> GetMissingParentColumns(ForeignKey fk)
        {
            return fk.References.Select(x => x.Column).Except(fk.ParentTable.Columns.Select(x => x.Name));
        }

        #endregion
    }
}