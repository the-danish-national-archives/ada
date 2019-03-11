namespace Ada.Checks.Table
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class TableIllegalEncoding : AdaAvXmlViolation
    {
        #region  Constructors

        public TableIllegalEncoding(XmlHandlerEventArgs args, string path)
            : base("4.D_14", args, path)
        {
        }

        #endregion
    }
}