//
//  <LogEntryType>
//    <EntryTypeId>4.A_1_4</EntryTypeId>
//    <EntryText>Fejl i primærnøglen. {TotalErrors} fejl i alt,  heraf {ErrorCount} ikke-unikke nøgler fordelt på { DistinctErrorCount }
//værdier, samt {NullErrorCount} nøgler indeholdende null-værdier, og {WhiteSpaceCount} nøgler med blanktegn ud af i alt {TotalRows} rækker({ ErrorPercentage} %)."</EntryText>
//    <Severity>1</Severity>
//  </LogEntryType>


//namespace Ada.Checks.PrimaryKey
//{
//    using System.Collections.Generic;
//
//    using Ada.ChecksBase;
//
//    using Ra.Common.ExtensionMethods;
//    using Ra.DomainEntities.TableIndex;
//
//    public class PrimaryKeyErrorSummary : AdaAvViolation
//    {
//        [AdaAvCheckNotificationTag]
//        public string TableName { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public string ConstraintName { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long TotalErrors { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long ErrorCount { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long DistinctErrorCount { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long NullErrorCount { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long WhiteSpaceCount { get; set; }
//
//        [AdaAvCheckNotificationTag]
//        public long TotalRows { get; set; }
//
//        [AdaAvCheckNotificationTagAsPercentage]
//        public float ErrorPercentage { get; set; }
//
//        private PrimaryKeyErrorSummary
//            (
//            PrimaryKey pk, 
//            long distinctRows, 
//            long distinctErrorRowsCount, 
//            long nullRowsCount, 
//            long whiteSpaceCount, 
//            long totalRows)
//            : base("4.A_1_4")
//        {
//            TableName = pk.ParentTableName;
//            ConstraintName = pk.Name;
//            ErrorCount = totalRows - distinctRows;
//            TotalErrors = ErrorCount + whiteSpaceCount + nullRowsCount;
//            DistinctErrorCount = distinctErrorRowsCount;
//            NullErrorCount = nullRowsCount;
//            WhiteSpaceCount = whiteSpaceCount;
//            TotalRows = totalRows;
//            ErrorPercentage = (float)TotalErrors / totalRows * 100;
//        }
//
//        public static IEnumerable<AdaAvCheckNotification> Create
//            (
//            PrimaryKey pk, 
//            long distinctRows, 
//            long distinctErrorRowsCount, 
//            long nullRowsCount, 
//            long whiteSpaceCount, 
//            long totalRows)
//        {
//            return (((totalRows - distinctRows) + whiteSpaceCount + nullRowsCount > 0)
//                        ? new PrimaryKeyErrorSummary(
//                              pk, 
//                              distinctRows, 
//                              distinctErrorRowsCount, 
//                              nullRowsCount, 
//                              whiteSpaceCount, 
//                              totalRows)
//                        : null)
//                .YieldOrEmpty();
//        }
//    }
//}

