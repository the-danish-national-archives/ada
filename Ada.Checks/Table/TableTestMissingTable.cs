namespace Ada.Checks.Table
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class TableTestMissingTable : AdaAvViolation
    {
        #region  Constructors

        public TableTestMissingTable(string tableName)
            : base("4.D_2")
        {
            TableName = tableName;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion
    }
}