namespace Ada.Checks.DocIndex
{
    #region Namespace Using

    using ChecksBase;
    using Ra.Common.Xml;

    #endregion

    public class DocIndexNotWellFormed : AdaAvXmlViolation
    {
        #region  Constructors

        public DocIndexNotWellFormed(XmlHandlerEventArgs args, string path)
            : base("4.C.1_9", args, path)
        {
        }

        #endregion
    }
}