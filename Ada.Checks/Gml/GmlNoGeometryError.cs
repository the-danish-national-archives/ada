namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlNoGeometryError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlNoGeometryError(string path)
            : base("5.G_28", path)
        {
        }

        #endregion
    }
}