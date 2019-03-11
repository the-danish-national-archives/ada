namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlGeometryOutOfBounds : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlGeometryOutOfBounds(string path)
            : base("5.G_29", path)
        {
        }

        #endregion
    }
}