namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class GmlNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public GmlNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("5.G_17", args, path)
        {
        }

        #endregion
    }
}