namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexTypeOriginalMissing : AdaAvViolation
    {
        #region  Constructors

        public TableIndexTypeOriginalMissing(int count)
            : base("6.C_17")
        {
            Count = count;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int Count { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(TableIndex index)
        {
            if (!string.IsNullOrEmpty(index.DbProduct))
            {
                var missingTypes = index.Tables.SelectMany(
                    table => table.Columns).Select(column => column.TypeOriginal).Count(string.IsNullOrEmpty);
                if (missingTypes > 0) yield return new TableIndexTypeOriginalMissing(missingTypes);
            }
        }

        #endregion
    }
}