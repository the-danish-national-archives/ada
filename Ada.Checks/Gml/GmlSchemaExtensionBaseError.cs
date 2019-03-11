namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaExtensionBaseError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaExtensionBaseError(string path)
            : base("5.G_12", path)
        {
        }

        #endregion
    }
}