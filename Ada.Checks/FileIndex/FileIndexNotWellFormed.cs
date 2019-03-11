namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class FileIndexNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public FileIndexNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("4.C.1_1", args, path)
        {
        }

        #endregion
    }
}