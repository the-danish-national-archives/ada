namespace Ada.Checks.Documents.ContextDocOnDisk
{
    public class ContextDocInvalidFirstNumber : XInvalidFirstNumber
    {
        #region  Constructors

        public ContextDocInvalidFirstNumber()
            : base("4.E_4", ContextDocToDiskContent.Instance)
        {
        }

        #endregion
    }
}