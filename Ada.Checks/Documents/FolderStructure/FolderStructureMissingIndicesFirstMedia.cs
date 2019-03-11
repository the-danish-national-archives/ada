namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureMissingIndicesFirstMedia : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureMissingIndicesFirstMedia()
            : base("4.B.2_1")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'Indices' AS folderName WHERE NOT EXISTS (SELECT * FROM foldersOnDisk WHERE name = 'Indices' AND mediaNumber = 1 AND depth=1)"
                ;
        }

        #endregion
    }
}