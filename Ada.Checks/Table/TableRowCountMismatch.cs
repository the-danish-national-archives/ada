namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using ChecksBase;

    #endregion

    public class TableRowCountMismatch : AdaAvViolation
    {
        #region  Constructors

        public TableRowCountMismatch(string tableName, long expectedRowCount, long actualRowCount)
            : base("6.C_26")
        {
            TableName = tableName;
            ExpectedRowCount = expectedRowCount;
            ActualRowCount = actualRowCount;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public long ActualRowCount { get; set; }

        [AdaAvCheckNotificationTag]
        public long ExpectedRowCount { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(string tableName, long expectedRowCount, long actualRowCount)
        {
            if (expectedRowCount != actualRowCount) yield return new TableRowCountMismatch(tableName, expectedRowCount, actualRowCount);
        }

        #endregion
    }
}