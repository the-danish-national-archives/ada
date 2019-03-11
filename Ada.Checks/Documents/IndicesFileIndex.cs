namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class IndicesFileIndex : AdaAvDynamicFromSql
    {
        #region  Constructors

        public IndicesFileIndex()
            : base("4.C_1")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'fileIndex.xml' WHERE NOT EXISTS (SELECT entryKey FROM indexFolderContent WHERE name = 'fileIndex.xml')";
        }

        #endregion
    }
}