namespace Ada.Checks.ContextDocIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class ContextDocumentationNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public ContextDocumentationNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("4.C.1_5", args, path)
        {
        }

        #endregion
    }
}