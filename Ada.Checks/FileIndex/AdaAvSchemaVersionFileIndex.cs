namespace Ada.Checks.FileIndex
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class AdaAvSchemaVersionFileIndex : AdaAvSchemaVersion
    {
        #region  Constructors

        public AdaAvSchemaVersionFileIndex(string fileName, string version)
            : base("4.F_6", fileName, version)
        {
        }

        #endregion
    }
}