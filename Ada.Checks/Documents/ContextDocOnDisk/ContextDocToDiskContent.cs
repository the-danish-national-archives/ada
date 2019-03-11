namespace Ada.Checks.Documents.ContextDocOnDisk
{
    public class ContextDocToDiskContent : XToDiskContent
    {
        #region Static

        #endregion

        #region  Constructors

        private ContextDocToDiskContent()
        {
        }

        #endregion

        #region Properties

        public override string CollectionsOnDisk => "contextDocumentationCollectionsOnDisk";
        public override string ContentOnDisk => "contextDocumentationDocumentContentOnDisk";
        public override string Documents => "contextDocumentationDocuments";

        public override string FolderContent => "contextDocumentationFolderContent";

        public override string FolderName => "ContextDocumentation";

        public static XToDiskContent Instance { get; } = new ContextDocToDiskContent();

        #endregion
    }
}