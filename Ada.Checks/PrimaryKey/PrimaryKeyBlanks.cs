namespace Ada.Checks.PrimaryKey
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

    public class PrimaryKeyBlanks : AdaAvViolation
    {
        #region  Constructors

        public PrimaryKeyBlanks(PrimaryKey pk, long whiteSpaceCount, long totalRows)
            : base("4.A_1_3")
        {
            TableName = pk.ParentTableName;
            ConstraintName = pk.Name;
            ColumnNames = pk.Columns;
            Count = whiteSpaceCount;
            TotalRows = totalRows;
            Percent = 100.0f * whiteSpaceCount / totalRows;
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

        public static IEnumerable<AdaAvCheckNotification> Check<TRepo>(PrimaryKey pk, TRepo sqlRepo) where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
        {
            var query = GetWhiteSpaceRowsQuery(pk);

            var whiteSpaceCount = sqlRepo.AsCountExecuteScalar(query);

            if (whiteSpaceCount == 0)
                yield break;

            var totalRows = sqlRepo.AsCountExecuteScalar($"{pk.ParentTable.Name}");

            yield return new PrimaryKeyBlanks(pk, whiteSpaceCount, totalRows);
        }

        [AdaAvCheckToAvQuery(nameof(ConstraintName))]
        public static string GetAvQuery(PrimaryKey pk)
        {
            return GetWhiteSpaceRowsQuery(pk);
        }

        private static string GetWhiteSpaceRowsQuery(PrimaryKey pk, int? limit = null)
        {
            var tableName = pk.ParentTable.Name;
            var columns = pk.Columns.SmartToString();

            var conditionalSeparator = " OR ";
            var nullList =
                pk.Columns.Select(
                        col =>
                            $"{col} LIKE ' %' OR {col} LIKE '% ' OR {col} LIKE '%'||X'09' OR {col} like X'09'||'%' OR {col} = ' ' OR {col} = X'09' ")
                    .ToList();
            var conditions = nullList.SmartToString(conditionalSeparator);

            var limitTemplate = limit != null ? $"LIMIT {limit.Value.ToString(CultureInfo.InvariantCulture)}" : string.Empty;

            var queryTemplate = $"SELECT {columns} FROM {tableName} WHERE {conditions} {limitTemplate}";

            return queryTemplate;
        }

        #endregion
    }
}