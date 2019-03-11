namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlSchemaNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlSchemaNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("5.G_5", args, path)
        {
        }

        #endregion
    }
}