namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureSchemasMissing : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureSchemasMissing()
            : base("4.B.2_2")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'Schemas' AS folderName WHERE NOT EXISTS (SELECT * FROM foldersOnDisk WHERE name = 'Schemas' AND mediaNumber = 1 AND depth=1)"
                ;
        }

        #endregion
    }
}