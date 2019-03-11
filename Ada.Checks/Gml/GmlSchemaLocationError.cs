namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaLocationError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaLocationError(string path)
            : base("5.G_19", path)
        {
        }

        #endregion
    }
}