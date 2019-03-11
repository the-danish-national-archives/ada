namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XMixedFileTypes : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XMixedFileTypes(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT relativePath as Path, COUNT(DISTINCT extension) from {_toDiskContent.ContentOnDisk}  GROUP by dID HAVING COUNT(DISTINCT extension) > 1"
                ;
        }

        #endregion
    }
}