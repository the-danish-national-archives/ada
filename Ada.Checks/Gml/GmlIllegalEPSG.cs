namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlIllegalEPSG : AdaAvGmlViolation
    {
        #region  Constructors

        public GmlIllegalEPSG(string path)
            : base("5.G_24", path)
        {
        }

        #endregion
    }
}