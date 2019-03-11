namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class IndicesDocIndex : AdaAvDynamicFromSql
    {
        #region  Constructors

        public IndicesDocIndex()
            : base("4.C_5")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'docIndex.xml' WHERE NOT EXISTS (SELECT entryKey FROM indexFolderContent WHERE name = 'docIndex.xml') AND EXISTS (SELECT * from mediaWithDocuments)";
        }

        #endregion
    }
}