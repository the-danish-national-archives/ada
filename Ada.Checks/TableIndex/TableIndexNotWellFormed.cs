namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class TableIndexNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public TableIndexNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("4.C.1_7", args, path)
        {
        }

        #endregion
    }
}