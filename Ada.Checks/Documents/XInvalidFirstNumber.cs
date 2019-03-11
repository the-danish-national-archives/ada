namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XInvalidFirstNumber : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XInvalidFirstNumber(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT COUNT(Missing) as Gaps ,'{_toDiskContent.FolderName}' AS Folder FROM 
												(SELECT entryKey as Missing FROM fsEntries WHERE entryKey <= (SELECT MAX(folderNumber) FROM {_toDiskContent.FolderContent})
												EXCEPT SELECT folderNumber as Missing FROM {_toDiskContent.FolderContent})
												GROUP BY Folder HAVING COUNT(Missing) > 0"
                ;
        }

        #endregion
    }
}