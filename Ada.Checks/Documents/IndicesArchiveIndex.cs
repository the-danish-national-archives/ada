namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class IndicesArchiveIndex : AdaAvDynamicFromSql
    {
        #region  Constructors

        public IndicesArchiveIndex()
            : base("4.C_2")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'archiveIndex.xml' WHERE NOT EXISTS (SELECT entryKey FROM indexFolderContent WHERE name = 'archiveIndex.xml')";
        }

        #endregion
    }
}