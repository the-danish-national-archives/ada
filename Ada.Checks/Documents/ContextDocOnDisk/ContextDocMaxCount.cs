namespace Ada.Checks.Documents.ContextDocOnDisk
{
    public class ContextDocMaxCount : XMaxCount
    {
        #region  Constructors

        public ContextDocMaxCount()
            : base("4.E_10", ContextDocToDiskContent.Instance)
        {
        }

        #endregion
    }
}