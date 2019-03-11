namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlFileTooLarge : AdaAvViolation
    {
        #region  Constructors

        public GmlFileTooLarge(string path)
            : base("5.G_16")
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