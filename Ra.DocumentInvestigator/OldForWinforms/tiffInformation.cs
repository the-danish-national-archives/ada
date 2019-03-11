namespace Ra.DocumentInvestigator.OldForWinforms
{
    #region Namespace Using

    using AdaAvChecking.Image;

    #endregion

    public struct TiffInformation
    {
        public bool IsTiff { get; set; }

        public int getPageCount { get; set; }

        public string LegalKompression { get; set; }

        public bool DPIOdd { get; set; }

        public int Fillorder { get; set; }

        public int PrivateTagsFound { get; set; }

        public BlankDokumentPagesResult BlankDokumentPagesPages { get; set; }

        public bool SideantalToHigh { get; set; }

        public string Filename { get; set; }
    }
}