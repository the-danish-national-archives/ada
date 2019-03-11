namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlInvalid(XmlHandlerEventArgs args, string path)
            : base("5.G_18", args, path)
        {
        }

        #endregion
    }
}