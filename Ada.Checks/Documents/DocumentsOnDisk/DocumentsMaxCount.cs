namespace Ada.Checks.Documents.DocumentsOnDisk
{
    public class DocumentsMaxCount : XMaxCount
    {
        #region  Constructors

        public DocumentsMaxCount()
            : base("4.G_11", DocumentsToDiskContent.Instance)
        {
        }

        #endregion
    }
}