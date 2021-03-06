namespace Ada.Checks.DocumentsContent
{
    #region Namespace Using

    using ChecksBase;
    using Ra.DomainEntities.Documents;

    #endregion

    public class DocumentsVideoContent : AdaAvViolation
    {
        #region  Constructors

        public DocumentsVideoContent(DocIndexEntry docInfo, string path)
            : base("5.F_4")
        {
            Path = path;
            DocumentID = docInfo.DocumentId;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string DocumentID { get; set; }

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion
    }
}