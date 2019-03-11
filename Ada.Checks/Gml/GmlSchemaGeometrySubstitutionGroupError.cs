namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaGeometrySubstitutionGroupError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlSchemaGeometrySubstitutionGroupError(string path)
            : base("5.G_10", path)
        {
        }

        #endregion
    }
}