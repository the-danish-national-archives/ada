namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class IndicesTableIndex : AdaAvDynamicFromSql
    {
        #region  Constructors

        public IndicesTableIndex()
            : base("4.C_4")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'tableIndex.xml' WHERE NOT EXISTS (SELECT entryKey FROM indexFolderContent WHERE name = 'tableIndex.xml')";
        }

        #endregion
    }
}