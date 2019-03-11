namespace Ada.Checks.Documents.DocumentsOnDisk
{
    public class DocumentsToDiskContent : XToDiskContent
    {
        #region Static

        #endregion

        #region  Constructors

        private DocumentsToDiskContent()
        {
        }

        #endregion

        #region Properties

        public override string CollectionsOnDisk => "documentsCollectionsOnDisk";
        public override string ContentOnDisk => "documentContentOnDisk";
        public override string Documents => "documents";

        public override string FolderContent => "documentFolderContent";

        public override string FolderName => "Documents";

        public static XToDiskContent Instance { get; } = new DocumentsToDiskContent();

        #endregion
    }
}