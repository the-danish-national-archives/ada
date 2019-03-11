namespace Ada.Checks.Documents.DocumentsOnDisk
{
    public class DocumentsBadFileTypes : XBadFileTypes
    {
        #region  Constructors

        public DocumentsBadFileTypes()
            : base("4.G_9", DocumentsToDiskContent.Instance)
        {
        }

        #endregion
    }
}