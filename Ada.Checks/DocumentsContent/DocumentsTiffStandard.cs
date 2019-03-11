namespace Ada.Checks.DocumentsContent
{
    #region Namespace Using

    using ChecksBase;
    using Ra.DomainEntities.Documents;

    #endregion

    public class DocumentsTiffStandard : AdaAvViolation
    {
        #region  Constructors

        public DocumentsTiffStandard(DocIndexEntry docInfo, string path)
            : base("5.E_2")
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