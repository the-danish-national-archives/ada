namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class DocIndexInvalidGmlXsd : AdaAvViolation
    {
        #region  Constructors

        public DocIndexInvalidGmlXsd(string path)
            : base("4.C.6_2")
        {
            Path = path;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Path { get; set; }

        #endregion
    }
}