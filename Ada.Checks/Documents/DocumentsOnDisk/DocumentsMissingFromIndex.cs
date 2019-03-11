namespace Ada.Checks.Documents.DocumentsOnDisk
{
    public class DocumentsMissingFromIndex : XMissingFromIndex
    {
        #region  Constructors

        public DocumentsMissingFromIndex()
            : base("4.G_7", DocumentsToDiskContent.Instance)
        {
        }

        #endregion
    }
}