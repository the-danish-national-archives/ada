namespace Ada.Checks.Table
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class TableNotValid : AdaAvXmlViolation
    {
        #region  Constructors

        public TableNotValid(XmlHandlerEventArgs args, string path)
            : base("4.D_9", args, path)
        {
            TableName = "";
            FolderNumber = args.fileInfo.DirectoryName;
            ErrorType = args.OriginalMessage;
            ErrorPlacement = "(" + args.Line + ", " + args.Position + ")";
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ErrorPlacement { get; set; }

        [AdaAvCheckNotificationTag]
        public string ErrorType { get; set; }

        [AdaAvCheckNotificationTag]
        public string FolderNumber { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion
    }
}