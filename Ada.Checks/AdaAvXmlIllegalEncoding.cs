namespace Ada.Checks
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class AdaAvXmlIndexIllegalEncoding : AdaAvXmlViolation
    {
        #region  Constructors

        public AdaAvXmlIndexIllegalEncoding(XmlHandlerEventArgs args, string path)
            : base("4.C.1_11", args, path)
        {
        }

        #endregion
    }
}