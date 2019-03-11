namespace Ada.Checks.Table
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class TableTestNotInIndex : AdaAvViolation
    {
        #region  Constructors

        public TableTestNotInIndex(string folderName)
            : base("4.D_1")
        {
            FolderName = folderName;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string FolderName { get; set; }

        #endregion
    }
}