namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class FileIndexInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public FileIndexInvalid(XmlHandlerEventArgs args, string path)
            : base("4.C.1_2", args, path)
        {
        }

        #endregion
    }
}