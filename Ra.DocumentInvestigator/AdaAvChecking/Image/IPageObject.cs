namespace Ra.DocumentInvestigator.AdaAvChecking.Image
{
    public interface IPageObject
    {
        #region Properties

        bool? BlankDokumentPage { get; set; }
        string Compression { get; set; }
        FillorderEnum Fillorder { get; set; }
        bool OddDBI { get; set; }
        int PageId { get; set; }
        int PageNo { get; set; }
        bool PrivateTagsInPage { get; set; }
        string TiffFileID { get; set; }

        #endregion
    }
}