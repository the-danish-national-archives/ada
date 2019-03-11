namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaGeometryMissingError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaGeometryMissingError(string path)
            : base("5.G_9", path)
        {
        }

        #endregion
    }
}