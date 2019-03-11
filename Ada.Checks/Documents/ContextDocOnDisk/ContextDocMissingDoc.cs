namespace Ada.Checks.Documents
{
    public class ContextDocMissingDoc : XMissingDoc
    {
        #region  Constructors

        public ContextDocMissingDoc()
            : base("4.E_6")
        {
        }

        #endregion

        #region Properties

        protected override string contentOnDisk => "contextDocumentationDocumentContentOnDisk";

        protected override string Documents => "contextDocumentationDocuments";

        #endregion
    }
}