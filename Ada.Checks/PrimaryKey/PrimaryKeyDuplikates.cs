namespace Ada.Checks.PrimaryKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Globalization;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class PrimaryKeyDuplikates : AdaAvViolation
    {
        #region  Constructors

        public PrimaryKeyDuplikates(PrimaryKey pk, long distinctRows, long totalRows)
            : base("4.A_1_1")
        {
            ConstraintName = pk.Name;
            TableName = pk.ParentTableName;
            ColumnNames = pk.Columns;
            Count = totalRows - distinctRows;
            TotalRows = totalRows;
            Percent = 100.0f * Count / TotalRows;
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

        public static IEnumerable<AdaAvCheckNotification> Check<TRepo>
        (
            PrimaryKey pk,
            TRepo sqlRepo) where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
        {
            var distinctRows = sqlRepo
                .AsCountExecuteScalar($"SELECT DISTINCT {pk.Columns.SmartToString()} FROM {pk.ParentTable.Name}");

            var totalRows = sqlRepo.AsCountExecuteScalar($"{pk.ParentTable.Name}");

            if (distinctRows == totalRows)
                yield break;

            yield return new PrimaryKeyDuplikates(pk, distinctRows, totalRows);
        }

        [AdaAvCheckToAvQuery(nameof(ConstraintName))]
        public static string GetAvQuery(PrimaryKey pk)
        {
            return GetDistinctErrorRowsQuery(pk);
        }

        private static string GetDistinctErrorRowsQuery(PrimaryKey pk, int? limit = null)
        {
            var limitPart = limit == null
                ? string.Empty
                : $"LIMIT {limit.Value.ToString(CultureInfo.InvariantCulture)}";

            var columns = pk.Columns.SmartToString();
            var tableName = pk.ParentTable.Name;

            var query =
                $"SELECT {columns}, COUNT (1) AS C FROM {tableName} GROUP BY {columns} HAVING C > 1 {limitPart}";

            return query;
        }

        #endregion
    }
}