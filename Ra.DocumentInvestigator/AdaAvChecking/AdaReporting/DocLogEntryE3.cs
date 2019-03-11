namespace Ra.DocumentInvestigator.AdaAvChecking.AdaReporting
{
    #region Namespace Using

    using Image;

    #endregion

    public class DocLogEntryE3 : DocLogEntryFromDocInfo
    {
        #region  Constructors

        public DocLogEntryE3(DocInfo docInfo, PageObject pageObject) : base(docInfo, "5.E_3")
        {
            AddTag("CompressionType", pageObject.Compression);
        }

        #endregion
    }
}