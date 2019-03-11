namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureTablesMissing : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureTablesMissing()
            : base("4.B.2_4")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'Tables' AS folderName WHERE NOT EXISTS (SELECT * FROM foldersOnDisk WHERE name = 'Tables' AND depth=1)"
                ;
        }

        #endregion
    }
}