namespace Ada.Checks.Documents.ContextDocOnDisk
{
    public class ContextDocMissingFromIndex : XMissingFromIndex
    {
        #region  Constructors

        public ContextDocMissingFromIndex()
            : base("4.E_7", ContextDocToDiskContent.Instance)
        {
        }

        #endregion
    }
}