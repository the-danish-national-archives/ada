namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaMissingFeature : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaMissingFeature(string path)
            : base("5.G_13", path)
        {
        }

        #endregion
    }
}