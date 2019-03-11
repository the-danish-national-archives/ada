namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class ArchiveIndexInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public ArchiveIndexInvalid(XmlHandlerEventArgs args, string path)
            : base("4.C.1_4", args, path)
        {
        }

        #endregion
    }
}