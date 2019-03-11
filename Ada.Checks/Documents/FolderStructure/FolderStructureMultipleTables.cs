namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureMultipleTables : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureMultipleTables()
            : base("4.B.2_9")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT mediaNumber as MedieNumber FROM foldersOnDisk WHERE name = 'Tables' AND depth=1"
                ;
        }

        #endregion
    }
}