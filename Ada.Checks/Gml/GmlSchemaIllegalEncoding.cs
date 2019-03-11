namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlSchemaIllegalEncoding : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlSchemaIllegalEncoding(XmlHandlerEventArgs args, string path)
            : base("5.G_6", args, path)
        {
        }

        #endregion
    }
}