namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class TableIndexReferencedTableMissing : AdaAvViolation
    {
        #region  Constructors

        public TableIndexReferencedTableMissing(ForeignKey fk)
            : base("6.C_19")
        {
            ConstraintName = fk.Name;
            TableName = fk.ParentTableName;
            ReferencedTable = fk.ReferencedTable;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ConstraintName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ReferencedTable { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ForeignKey fk)
        {
            if (fk.GetReferencedTable() == null) yield return new TableIndexReferencedTableMissing(fk);
        }

        #endregion
    }
}