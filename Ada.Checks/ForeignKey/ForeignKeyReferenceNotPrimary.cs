namespace Ada.Checks.ForeignKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class ForeignKeyReferenceNotPrimary : AdaAvViolation
    {
        #region  Constructors

        public ForeignKeyReferenceNotPrimary(ForeignKey fk, Reference nonRef)
            : base("4.A_1_7")
        {
            TableName = fk.ParentTableName;
            ReferencedTable = fk.ReferencedTable;
            ConstraintName = fk.Name;
            Column = nonRef.Referenced;
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
            var nonRefs = GetNonPrimaryReferences(fk);
            foreach (var nonRef in nonRefs) yield return new ForeignKeyReferenceNotPrimary(fk, nonRef);
        }

        private static IEnumerable<Reference> GetNonPrimaryReferences(ForeignKey fk)
        {
            return fk.References.Where(rf => !ReferencedFieldIsPrimaryKeyField(rf)).ToList();
        }

        private static bool ReferencedFieldIsPrimaryKeyField(Reference reference)
        {
            var referencedTable = reference.ParentConstraint.GetReferencedTable();
            return referencedTable != null && referencedTable.PrimaryKey.Columns.Contains(reference.Referenced);
        }

        #endregion
    }
}