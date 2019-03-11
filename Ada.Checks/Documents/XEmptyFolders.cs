namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XEmptyFolders : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XEmptyFolders(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT name AS FolderName FROM {_toDiskContent.FolderContent} WHERE name LIKE 'docCollection%' AND entryType='Dir' AND folderCount = 0"
                ;
        }

        #endregion
    }
}