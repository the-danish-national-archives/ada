namespace Ada.Checks.ContextDocIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class ContextDocumentationInvalid : AdaAvXmlViolation
    {
        #region  Constructors

        public ContextDocumentationInvalid(XmlHandlerEventArgs args, string path)
            : base("4.C.1_6", args, path)
        {
        }

        #endregion
    }
}