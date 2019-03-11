namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class SchemaMissingFolder : AdaAvDynamicFromSql
    {
        #region  Constructors

        public SchemaMissingFolder()
            : base("4.F_1")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT 'localShared' AS folderName WHERE NOT EXISTS (SELECT name FROM schemaFolders WHERE name = 'localShared' )
												UNION ALL
												SELECT 'standard' AS folderName WHERE NOT EXISTS (SELECT name FROM schemaFolders WHERE name = 'standard' )"
                ;
        }

        #endregion
    }
}