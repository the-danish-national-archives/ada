namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class DocIndexInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public DocIndexInvalid(XmlHandlerEventArgs args, string path)
            : base("4.C.1_10", args, path)
        {
        }

        #endregion
    }
}