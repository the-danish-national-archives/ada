namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class IndicesContextDocIndex : AdaAvDynamicFromSql
    {
        #region  Constructors

        public IndicesContextDocIndex()
            : base("4.C_3")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'contextDocumentationIndex.xml' WHERE NOT EXISTS (SELECT entryKey FROM indexFolderContent WHERE name = 'contextDocumentationIndex.xml')";
        }

        #endregion
    }
}