namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaMissingFeatureAnnotation : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaMissingFeatureAnnotation(string path)
            : base("5.G_14", path)
        {
        }

        #endregion
    }
}