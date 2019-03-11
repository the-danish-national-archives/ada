namespace Ada.Checks.ForeignKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class ForeignKeyTypeMismatch : AdaAvViolation
    {
        #region  Constructors

        public ForeignKeyTypeMismatch(ForeignKey fk, Reference reference, string colType, string refType)
            : base("4.A_1_8")
        {
            ConstraintName = fk.Name;
            TableName = fk.ParentTableName;
            ReferencedTable = fk.ReferencedTable;
            Column = reference.Column;
            ReferencedColumn = reference.Referenced;
            ColumnType = colType;
            ReferencedColumnType = refType;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Column { get; set; }

        [AdaAvCheckNotificationTag]
        public string ColumnType { get; set; }

        [AdaAvCheckNotificationTag]
        public string ConstraintName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ReferencedColumn { get; set; }

        [AdaAvCheckNotificationTag]
        public string ReferencedColumnType { get; set; }

        [AdaAvCheckNotificationTag]
        public string ReferencedTable { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(ForeignKey fk)
        {
            foreach (var reference in fk.References)
            {
                var colType = ConstraintFieldType(reference);
                var refType = ReferencedFieldType(reference);

                if (colType.Equals(refType))
                    continue;

                yield return new ForeignKeyTypeMismatch(fk, reference, colType, refType);
            }
        }

        private static bool ConstraintFieldExists(Reference reference)
        {
            var constraintTable = reference.ParentConstraint.ParentTable;
            return constraintTable.Columns.ToList().Exists(x => x.Name.Equals(reference.Column));
        }

        private static string ConstraintFieldType(Reference reference)
        {
            if (ConstraintFieldExists(reference))
                return
                    reference.ParentConstraint.ParentTable.Columns.ToList()
                        .Find(x => x.Name.Equals(reference.Column))
                        .GetXmlMappedType();

            return null;
        }

        private static bool ReferencedFieldExists(Reference reference)
        {
            var referencedTable = reference.ParentConstraint.GetReferencedTable();
            return referencedTable != null
                   && referencedTable.Columns.ToList().Exists(x => x.Name.Equals(reference.Referenced));
        }

        private static string ReferencedFieldType(Reference reference)
        {
            if (ReferencedFieldExists(reference))
                return
                    reference.ParentConstraint.GetReferencedTable()
                        .Columns.ToList()
                        .Find(x => x.Name.Equals(reference.Referenced))
                        .GetXmlMappedType();

            return null;
        }

        #endregion
    }
}