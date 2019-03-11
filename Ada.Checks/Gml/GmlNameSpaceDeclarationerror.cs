namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlNameSpaceDeclarationerror : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlNameSpaceDeclarationerror(string path)
            : base("5.G_23", path)
        {
        }

        #endregion
    }
}