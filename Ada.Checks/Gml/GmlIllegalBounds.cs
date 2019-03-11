namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlIllegalBounds : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlIllegalBounds(string path)
            : base("5.G_26", path)
        {
        }

        #endregion
    }
}