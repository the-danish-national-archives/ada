namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlRootElementError : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlRootElementError(string path)
            : base("5.G_22", path)
        {
        }

        #endregion
    }
}