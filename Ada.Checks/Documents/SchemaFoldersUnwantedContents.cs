namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class SchemaFoldersUnwantedContents : AdaAvDynamicFromSql
    {
        #region  Constructors

        public SchemaFoldersUnwantedContents()
            : base("4.F_2")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT name as Name, 'standard' as Folder from standardSchemasOnDisk
                EXCEPT 
                SELECT name, 'standard' FROM indexSchemas
                UNION ALL
                SELECT name, 'localShared' from sharedSchemasOnDisk WHERE name NOT LIKE 'localSchema%.xsd'
                EXCEPT 
                SELECT name, 'localShared' FROM gisSchemas
                UNION ALL
                SELECT name, 'schemas' from schemaFolders WHERE name != 'localShared' AND name != 'standard'";
        }

        #endregion
    }
}