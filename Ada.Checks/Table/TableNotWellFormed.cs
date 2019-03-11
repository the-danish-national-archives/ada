namespace Ada.Checks.Table
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class TableNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public TableNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("4.D_8", args, path)
        {
        }

        #endregion
    }
}