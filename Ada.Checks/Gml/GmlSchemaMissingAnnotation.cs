namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaMissingAnnotation : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaMissingAnnotation(string path)
            : base("5.G_11", path)
        {
        }

        #endregion
    }
}