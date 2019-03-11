namespace Ra.DocumentInvestigator.AdaAvChecking.AdaReporting
{
    public class DocLogEntryE13 : DocLogEntryFromDocInfo
    {
        #region  Constructors

        public DocLogEntryE13(DocInfo docInfo, uint pageCount) : base(docInfo, "5.E_13")
        {
            AddTag("PageCount", pageCount.ToString());
        }

        #endregion
    }
}