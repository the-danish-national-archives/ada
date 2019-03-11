namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XMissingFromIndex : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XMissingFromIndex(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT dID as DocID, relativePath as Path from {_toDiskContent.ContentOnDisk} WHERE dID NOT IN (SELECT dID FROM {_toDiskContent.Documents})";
        }

        #endregion
    }
}