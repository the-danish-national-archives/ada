namespace Ada.Checks.Documents.ContextDocOnDisk
{
    public class ContextDocInvalidObject : XInvalidObject
    {
        #region  Constructors

        public ContextDocInvalidObject()
            : base("4.E_1", ContextDocToDiskContent.Instance)
        {
        }

        #endregion
    }
}