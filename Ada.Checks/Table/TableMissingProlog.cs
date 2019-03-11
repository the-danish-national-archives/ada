namespace Ada.Checks.Table
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class TableMissingProlog : AdaAvXmlViolation
    {
        #region  Constructors

        public TableMissingProlog(XmlHandlerEventArgs args, string path)
            : base("4.D_13", args, path)
        {
        }

        #endregion
    }
}