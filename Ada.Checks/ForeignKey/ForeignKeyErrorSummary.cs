//
//  <LogEntryType>
//    <EntryTypeId>4.A_1_9</EntryTypeId>
//    <EntryText>Fremmednøglefejl i tabel {TableName}. Fejl i fremmednøglen {ConstraintName}. {TotalErrors} fejl fordelt på {DistinctErrors} unikke værdier ud af i alt { RowCount } rækker({ ErrorPercentage}%).</EntryText>
//    <Severity>1</Severity>
//  </LogEntryType>


//namespace Ada.Checks.ForeignKey
//{
//    using System.Collections.Generic;
//    using System.Linq;
//
//    using Ada.ChecksBase;
//    using Ada.Repositories;
//
//    using Ra.Common.ExtensionMethods;
//    using Ra.DomainEntities.TableIndex;
//
//    public class ForeignKeyErrorSummary : AdaAvViolation
//    {
//        public ForeignKeyErrorSummary(ForeignKey fk, long errorcount, long distinctCount, long rowcount)
//            : base("4.A_1_9")
//        {
//            var errpercentage = (rowcount > 0) ? (decimal)errorcount / rowcount * 100 : 0;
//            ErrorPercentage = (float)errpercentage;
//            ConstraintName = fk.Name;
//            TableName = fk.ParentTableName;
//            TotalErrors = errorcount;
//            DistinctErrors = distinctCount;
//            RowCount = rowcount;
//        }
//
//        [AdaAvCheckNotificationTagAsPercentage]
//        public float ErrorPercentage { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public string ConstraintName { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public string TableName { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long TotalErrors { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long DistinctErrors { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long RowCount { get; set; }
//
//        public static IEnumerable<AdaAvCheckNotification> Check<TRepo>
//            (ForeignKey fk, TRepo sqlRepo, long rowcount, long emptyfkcount, long distinctCount)
//            where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
//        {
//            var matchcount = sqlRepo.ExecuteScalar(GetActualMatchesQuery(fk));
//
//            long errorcount = rowcount - matchcount - emptyfkcount;
//
//            return (errorcount > 0
//                        ? new ForeignKeyErrorSummary(fk, errorcount, distinctCount, rowcount)
//                        : null)
//                .YieldOrEmpty();
//        }
//
//        private static string GetActualMatchesQuery(ForeignKey fk)
//        {
//            var tableName = fk.ParentTableName;
//            var referencedTable = fk.ReferencedTable;
//
//            var criteria =
//                fk.References.Select(
//                    x =>
//                    $"A.{x.Column} = B.{x.Referenced}").SmartToString(" AND ");
//
//            var queryTemplate = $"SELECT COUNT(DISTINCT A.rowid) FROM {tableName} AS A"
//                                + $" INNER JOIN {referencedTable} AS B ON {criteria}";
//
//            return queryTemplate;
//        }
//    }
//}

