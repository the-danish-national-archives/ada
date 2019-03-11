namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XMaxCount : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XMaxCount(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                //                @"SELECT name AS FolderName, folderCount as Count FROM contextDocumentationCollectionsOnDisk WHERE folderCount > 10000";
                $@"SELECT name AS FolderName, folderCount as Count FROM {_toDiskContent.FolderContent} WHERE folderCount > 10000";
        }

        #endregion
    }
}