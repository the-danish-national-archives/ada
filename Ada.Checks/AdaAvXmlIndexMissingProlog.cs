namespace Ada.Checks
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class AdaAvXmlIndexMissingProlog : AdaAvXmlViolation
    {
        #region  Constructors

        public AdaAvXmlIndexMissingProlog(XmlHandlerEventArgs args, string path)
            : base("4.C.1_12", args, path)
        {
        }

        #endregion
    }
}