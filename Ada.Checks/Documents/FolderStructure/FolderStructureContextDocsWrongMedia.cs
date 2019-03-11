namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureContextDocsWrongMedia : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureContextDocsWrongMedia()
            : base("4.B.2_7")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT mediaNumber as MedieNumber FROM foldersOnDisk WHERE name = 'ContextDocumentation' AND mediaNumber != 1 AND depth=1"
                ;
        }

        #endregion
    }
}