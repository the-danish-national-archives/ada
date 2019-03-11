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

    public class ForeignKeyBlanks : AdaAvViolation
    {
        #region  Constructors

        public ForeignKeyBlanks(ForeignKey fk, long whiteSpaceCount, long totalRows)
            : base("4.A_1_10")
        {
            ConstraintName = fk.Name;
            TableName = fk.ParentTableName;
            ColumnNames = fk.References.Select(x => x.Column);
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

        public static IEnumerable<AdaAvCheckNotification> Check<TRepo>(ForeignKey fk, TRepo sqlRepo) where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
        {
            var query = GetWhiteSpaceRowsQuery(fk);

            var whiteSpaceCount = sqlRepo.AsCountExecuteScalar(query);

            if (whiteSpaceCount == 0)
                yield break;

            var totalRows = sqlRepo.AsCountExecuteScalar($"{fk.ParentTable.Name}");

            yield return new ForeignKeyBlanks(fk, whiteSpaceCount, totalRows);
        }

        [AdaAvCheckToAvQuery(nameof(ConstraintName))]
        public static string GetAvQuery(ForeignKey fk)
        {
            return GetWhiteSpaceRowsQuery(fk);
        }

        private static string GetWhiteSpaceRowsQuery(ForeignKey fk, int? limit = null)
        {
            var columns = fk.References.Select(x => x.Column).SmartToString();
            var tableName = fk.ParentTableName;

            var conditionalSeparator = " OR ";
            var nullList =
                fk.References.Select(x => $"{x.Column} LIKE ' %' OR {x.Column} LIKE '% '"
                                          + $" OR {x.Column} LIKE '%'||X'09' OR {x.Column} like X'09'||'%' OR" +
                                          $" {x.Column} = ' ' OR {x.Column} = X'09' ")
                    .ToList();
            var conditions = nullList.SmartToString(conditionalSeparator);

            var limitTemplate = limit != null ? $"LIMIT {limit.Value.ToString(CultureInfo.InvariantCulture)}" : string.Empty;

            var queryTemplate = $"SELECT {columns} FROM {tableName} WHERE {conditions} {limitTemplate}";

            return queryTemplate;
        }

        #endregion
    }
}