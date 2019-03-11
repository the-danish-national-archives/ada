namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlIllegalEncoding : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlIllegalEncoding(XmlHandlerEventArgs args, string path)
            : base("5.G_20", args, path)
        {
        }

        #endregion
    }
}