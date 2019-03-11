namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureSchemasWrongMedia : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureSchemasWrongMedia()
            : base("4.B.2_6")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT mediaNumber as MedieNumber FROM foldersOnDisk WHERE name = 'Schemas' AND mediaNumber != 1 AND depth=1"
                ;
        }

        #endregion
    }
}