namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class AdaAvSchemaVersionXmlSchema : AdaAvSchemaVersion
    {
        #region  Constructors

        public AdaAvSchemaVersionXmlSchema(string fileName, string version)
            : base("4.F_8", fileName, version)
        {
        }

        #endregion
    }
}