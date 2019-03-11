namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class AdaAvSchemaVersionTableIndex : AdaAvSchemaVersion
    {
        #region  Constructors

        public AdaAvSchemaVersionTableIndex(string fileName, string version)
            : base("4.F_5", fileName, version)
        {
        }

        #endregion
    }
}