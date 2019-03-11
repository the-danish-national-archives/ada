namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureFirstMediaMissing : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureFirstMediaMissing()
            : base("4.B.1_1")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return @"SELECT '1' AS MediaNumber WHERE NOT EXISTS (SELECT * FROM media WHERE mediaNumber = '1')"
                ;
        }

        #endregion
    }
}