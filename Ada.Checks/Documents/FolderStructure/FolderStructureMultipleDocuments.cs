namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureMultipleDocuments : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureMultipleDocuments()
            : base("4.B.2_8")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT mediaNumber as MedieNumber FROM foldersOnDisk WHERE name = 'Documents' AND depth=1"
                ;
        }

        #endregion
    }
}