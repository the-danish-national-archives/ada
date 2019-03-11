namespace Ada.Checks.Documents.ContextDocOnDisk
{
    public class ContextDocBadFileTypes : XBadFileTypes
    {
        #region  Constructors

        public ContextDocBadFileTypes()
            : base("4.E_9", ContextDocToDiskContent.Instance)
        {
        }

        #endregion
    }
}