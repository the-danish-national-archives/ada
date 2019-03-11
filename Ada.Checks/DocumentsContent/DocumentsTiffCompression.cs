namespace Ada.Checks.DocumentsContent
{
    #region Namespace Using

    using ChecksBase;
    using Ra.DomainEntities.Documents;

    #endregion

    public class DocumentsTiffCompression : AdaAvViolation
    {
        #region  Constructors

        public DocumentsTiffCompression(DocIndexEntry docInfo, string path, string compressionType)
            : base("5.E_3")
        {
            Path = path;
            DocumentID = docInfo.DocumentId;
            if (compressionType == "Tif") // tif is simply a general description, giving no good information
                compressionType = "Unknown";
            CompressionType = compressionType;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string CompressionType { get; set; }

        [AdaAvCheckNotificationTag]
        public string DocumentID { get; set; }

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion
    }
}