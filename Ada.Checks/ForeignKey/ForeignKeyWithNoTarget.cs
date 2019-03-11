namespace Ada.Checks.ForeignKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class ForeignKeyWithNoTarget : AdaAvViolation
    {
        #region  Constructors

        public ForeignKeyWithNoTarget(ForeignKey fk, long noTargetRows, long totalRows)
            : base("4.A_1_5")
        {
            ConstraintName = fk.Name;
            TableName = fk.ParentTableName;
            ColumnNames = fk.References.Select(x => x.Column);
            Count = noTargetRows;
            TotalRows = totalRows;
            Percent = 100.0f * noTargetRows / totalRows;
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
            (ForeignKey fk, TRepo sqlRepo) where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
        {
            var noTargetRows = sqlRepo.AsCountExecuteScalar(GetDistinctErrorRowsQuery(fk));

            if (noTargetRows == 0)
                yield break;

            var totalRows = sqlRepo.AsCountExecuteScalar($"{fk.ParentTable.Name}");


            yield return new ForeignKeyWithNoTarget(fk, noTargetRows, totalRows);
        }

        [AdaAvCheckToAvQuery(nameof(ConstraintName))]
        public static string GetAvQuery(ForeignKey fk)
        {
            return GetDistinctErrorRowsQuery(fk);
        }

        private static string GetDistinctErrorRowsQuery(ForeignKey fk, int? limit = null)
        {
            var columns = fk.References.Select(x => x.Column).ToList();

            var constraintColumns = columns.SmartToString();

            var tableName = fk.ParentTableName;

            var whereClause = columns.Select(c => "(" + c + " IS NOT NULL AND " + c + " <> '')").SmartToString(" AND ");

            var referencedColumns = fk.References.Select(x => x.Referenced).ToList().SmartToString();
            var referencedTable = fk.ReferencedTable;

            var limitTemplate = limit != null
                ? $"LIMIT {limit.Value.ToString(CultureInfo.InvariantCulture)}"
                : string.Empty;

            var queryTemplate =
                $"SELECT {constraintColumns} FROM {tableName} WHERE {whereClause}"
                + $" EXCEPT SELECT {referencedColumns} FROM {referencedTable} {limitTemplate}";

            return queryTemplate;
        }

        #endregion
    }
}