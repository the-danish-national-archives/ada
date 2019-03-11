namespace Ada.Checks.Documents.DocumentsOnDisk
{
    public class DocumentsInvalidFirstNumber : XInvalidFirstNumber
    {
        #region  Constructors

        public DocumentsInvalidFirstNumber()
            : base("4.G_4", DocumentsToDiskContent.Instance)
        {
        }

        #endregion
    }
}