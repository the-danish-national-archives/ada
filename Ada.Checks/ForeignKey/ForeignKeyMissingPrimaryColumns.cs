namespace Ada.Checks.ForeignKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class ForeignKeyMissingPrimaryColumns : AdaAvViolation
    {
        #region  Constructors

        public ForeignKeyMissingPrimaryColumns(ForeignKey fk, string missingRef)
            : base("4.A_1_6")
        {
            TableName = fk.ParentTableName;
            ReferencedTable = fk.ReferencedTable;
            ConstraintName = fk.Name;
            Column = missingRef;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Column { get; set; }

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
            var missingRefs = GetMissingPrimaryColumns(fk);
            foreach (var missingRef in missingRefs) yield return new ForeignKeyMissingPrimaryColumns(fk, missingRef);
        }

        private static IEnumerable<string> GetMissingPrimaryColumns(ForeignKey fk)
        {
            return fk.GetReferencedTable().PrimaryKey.Columns.Except(fk.References.Select(x => x.Referenced));
        }

        #endregion
    }
}