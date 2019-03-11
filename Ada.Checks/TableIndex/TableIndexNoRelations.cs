namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class TableIndexNoRelations : AdaAvViolation
    {
        #region  Constructors

        public TableIndexNoRelations(IEnumerable<string> tableNames)
            : base("4.C.5_1")
        {
            TableNames = tableNames.ToList();
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTagSmartToString(Seperator = ", ")]
        //"¤¤")]
        public IEnumerable<string> TableNames { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(TableIndex index)
        {
            var referedTables = index.AllForeignKeys().Select(f => f.ReferencedTable).Distinct();

            var refferingTables = index.AllForeignKeys().Select(f => f.ParentTableName).Distinct();

            var tablesWithNoRelations = index.Tables.Select(t => t.Name).Except(referedTables.Union(refferingTables)).ToList();

            if (tablesWithNoRelations.Count != 0)
                yield return new TableIndexNoRelations(tablesWithNoRelations);
        }

        #endregion
    }
}