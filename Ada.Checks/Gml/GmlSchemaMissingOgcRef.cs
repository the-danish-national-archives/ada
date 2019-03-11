namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaMissingOgcRef : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaMissingOgcRef(string path)
            : base("5.G_15", path)
        {
        }

        #endregion
    }
}