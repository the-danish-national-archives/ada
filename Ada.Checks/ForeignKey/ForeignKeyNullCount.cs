namespace Ada.Checks.ForeignKey
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class ForeignKeyNullCount : AdaAvViolation
    {
        #region  Constructors

        public ForeignKeyNullCount(ForeignKey fk, long nullCount, long totalRows)
            : base("4.A_1_11")
        {
            TableName = fk.ParentTableName;
            ConstraintName = fk.Name;
            ColumnNames = fk.References.Select(x => x.Column);
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

        public static IEnumerable<AdaAvCheckNotification> Check<TRepo>
            (ForeignKey fk, TRepo sqlRepo)
            where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
        {
            var nullCount = sqlRepo.AsCountExecuteScalar(GetNullRowsQuery(fk));

            if (nullCount == 0)
                yield break;

            var totalRows = sqlRepo.ExecuteScalar(GetTotalRowsQuery(fk));

            yield return new ForeignKeyNullCount(fk, nullCount, totalRows);
        }

        [AdaAvCheckToAvQuery(nameof(ConstraintName))]
        public static string GetAvQuery(ForeignKey fk)
        {
            return GetNullRowsQuery(fk);
        }

        private static string GetNullRowsQuery(ForeignKey fk)
        {
            var columns = fk.References.Select(x => x.Column).SmartToString();
            var tableName = fk.ParentTableName;


            var nullList = fk.References.Select(x => $"{x.Column} IS NULL OR {x.Column} = ''");
            var conditionalSeparator = " OR ";
            var conditions = nullList.SmartToString(conditionalSeparator);

            var queryTemplate = $"SELECT {columns} FROM {tableName} WHERE {conditions}";

            return queryTemplate;
        }

        private static string GetTotalRowsQuery(ForeignKey fk)
        {
            var tableName = fk.ParentTableName;
            var queryTemplate = $"SELECT COUNT(1) FROM {tableName}";
            return queryTemplate;
        }

        #endregion
    }
}