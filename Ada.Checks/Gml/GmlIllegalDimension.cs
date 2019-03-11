namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlIllegalDimension : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlIllegalDimension(string path)
            : base("5.G_25", path)
        {
        }

        #endregion
    }
}