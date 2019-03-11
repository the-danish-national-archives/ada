namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaOgcImportError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaOgcImportError(string path)
            : base("5.G_8", path)
        {
        }

        #endregion
    }
}