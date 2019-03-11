namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XDuplicateFolderName : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XDuplicateFolderName(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"select dID as DocumentID, COUNT(dID) as Count FROM 
            		                    (select dID, mediaNumber, relativePath FROM {_toDiskContent.ContentOnDisk} GROUP BY dID, mediaNumber, relativePath)
            		                    GROUP BY dID HAVING COUNT(dID) > 1";
        }

        #endregion
    }
}