namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class TableIndexInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public TableIndexInvalid(XmlHandlerEventArgs args, string path)
            : base("4.C.1_8", args, path)
        {
        }

        #endregion
    }
}