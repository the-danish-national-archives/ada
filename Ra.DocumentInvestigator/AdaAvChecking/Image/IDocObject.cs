namespace Ra.DocumentInvestigator.AdaAvChecking.Image
{
    #region Namespace Using

    using filetypes;

    #endregion

    public interface IDocObject
    {
        #region Properties

        //       bool IsTiff { get; }
        LegalFileType FileType { get; }
        bool FirstPageBlank { get; }
        bool HaveOddDBI { get; }
        bool LegalCompression { get; }
        string Name { get; }
        uint PageCount { get; }
        bool PrivateTagsFound { get; }

        #endregion
    }
}