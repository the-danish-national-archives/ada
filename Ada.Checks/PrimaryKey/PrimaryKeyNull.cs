namespace Ada.Checks.PrimaryKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class PrimaryKeyNull : AdaAvViolation
    {
        #region  Constructors

        public PrimaryKeyNull(PrimaryKey pk, long nullCount, long totalRows)
            : base("4.A_1_2")
        {
            ConstraintName = pk.Name;
            TableName = pk.ParentTableName;
            ColumnNames = pk.Columns;
            Count = nullCount;
            TotalRows = totalRows;
            Percent = 100.0f * nullCount / totalRows;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTagSmartToString]
        public IEnumerable<string> ColumnNames { get; set; }

        [AdaAvCheckNotificationTag]
        public string ConstraintName { get; set; }

        [AdaAvCheckNotificationTag]
        public long Count { get; set; }

        [AdaAvCheckNotificationTagAsPercentage]
        public float Percent { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        [AdaAvCheckNotificationTag]
        public long TotalRows { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check<TRepo>(PrimaryKey pk, TRepo sqlRepo)
            where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
        {
            var nullCount = sqlRepo.AsCountExecuteScalar(GetNullRowsQuery(pk));

            if (nullCount == 0)
                yield break;

            var totalRows = sqlRepo.AsCountExecuteScalar($"{pk.ParentTable.Name}");


            yield return new PrimaryKeyNull(pk, nullCount, totalRows);
        }

        [AdaAvCheckToAvQuery(nameof(ConstraintName))]
        public static string GetAvQuery(PrimaryKey pk)
        {
            return GetNullRowsQuery(pk);
        }

        private static string GetNullRowsQuery(PrimaryKey pk)
        {
            var columns = pk.Columns.SmartToString();
            var tableName = pk.ParentTable.Name;

            var nullList = pk.Columns.Select(col => $"{col} IS NULL OR {col} = ''");
            var conditionalSeparator = " OR ";
            var conditions = nullList.SmartToString(conditionalSeparator);

            var queryTemplate = $"SELECT {columns} FROM {tableName} WHERE {conditions}";


            return queryTemplate;
        }

        #endregion
    }
}