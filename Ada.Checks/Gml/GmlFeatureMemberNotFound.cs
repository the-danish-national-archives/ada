namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlFeatureMemberNotFound : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlFeatureMemberNotFound(string path)
            : base("5.G_27", path)
        {
        }

        #endregion
    }
}