namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlSchemaInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlSchemaInvalid(XmlHandlerEventArgs args, string path)
            : base("5.G_4", args, path)
        {
        }

        #endregion
    }
}