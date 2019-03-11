namespace Ada.Checks.ArchiveIndex
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class AdaAvSchemaVersionArchieIndex : AdaAvSchemaVersion
    {
        #region  Constructors

        public AdaAvSchemaVersionArchieIndex(string fileName, string version)
            : base("4.F_3", fileName, version)
        {
        }

        #endregion
    }
}