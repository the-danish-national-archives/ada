namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class ArchiveIndexNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public ArchiveIndexNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("4.C.1_3", args, path)
        {
        }

        #endregion
    }
}