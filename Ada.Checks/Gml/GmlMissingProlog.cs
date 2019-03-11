namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlMissingProlog : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlMissingProlog(XmlHandlerEventArgs args, string path)
            : base("5.G_31", args, path)
        {
        }

        #endregion
    }
}