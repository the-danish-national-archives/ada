namespace Ada.Checks.Documents.FolderStructure
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class FolderStructureIndicesWrongMedia : AdaAvDynamicFromSql
    {
        #region  Constructors

        public FolderStructureIndicesWrongMedia()
            : base("4.B.2_5")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT mediaNumber as MedieNumber FROM foldersOnDisk WHERE name = 'Indices' AND mediaNumber != 1 AND depth=1"
                ;
        }

        #endregion
    }
}