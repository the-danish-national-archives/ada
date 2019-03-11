namespace Ada.Checks.Documents.ContextDocOnDisk
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class ContextDocEmptyDocFolder : AdaAvDynamicFromSql
    {
        #region  Constructors

        public ContextDocEmptyDocFolder()
            : base("4.E_14")
        {
        }

        #endregion

        #region Properties

        private string AllDocFolders => "contextDocumentationCollectionsOnDisk";

        private string DocFoldersWithContent => "contextDocumentationDocumentContentOnDisk";

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return $@"SELECT relativePath || '\' || Name AS Path  FROM {AllDocFolders} WHERE entryType = 'Dir'
                    EXCEPT
                    SELECT relativePath AS Path FROM {DocFoldersWithContent}";
        }

        #endregion
    }
}