namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureContextDocsMissing : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureContextDocsMissing()
            : base("4.B.2_3")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'ContextDocumentation' AS folderName WHERE NOT EXISTS (SELECT * FROM foldersOnDisk WHERE name = 'ContextDocumentation' AND mediaNumber = 1 AND depth=1)"
                ;
        }

        #endregion
    }
}