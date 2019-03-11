namespace Ada.Checks.DocumentsContent
{
    #region Namespace Using

    using ChecksBase;
    using Ra.DomainEntities.Documents;

    #endregion

    public class DocumentsHighPageCount : AdaAvViolation
    {
        #region  Constructors

        public DocumentsHighPageCount(DocIndexEntry docInfo, string path, string pageCount)
            : base("5.E_13")
        {
            Path = path;
            DocumentID = docInfo.DocumentId;
            int pageCountInt;
            PageCount = int.TryParse(pageCount, out pageCountInt) ? pageCountInt : 0;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string DocumentID { get; set; }

        [AdaAvCheckNotificationTag]
        public int PageCount { get; set; }

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion
    }
}