namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XNoFolders : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XNoFolders(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT '\{_toDiskContent.FolderName}' as FolderName from"
                + @"(SELECT 'X' WHERE NOT EXISTS "
                + $@"(SELECT entryKey FROM {_toDiskContent.FolderContent} WHERE name LIKE 'docCollection%'))"
                + @"join "
                + $@"(SELECT 'X' from foldersOnDisk WHERE depth = 1 AND name = '{_toDiskContent.FolderName}')"
                ;
        }

        #endregion
    }
}