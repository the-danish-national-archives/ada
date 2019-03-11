namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XMissingDoc : AdaAvDynamicFromSql
    {
        #region  Constructors

        protected XMissingDoc(string tagType)
            : base(tagType)
        {
        }

        #endregion

        #region Properties

        protected abstract string contentOnDisk { get; }

        protected abstract string Documents { get; }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT dID as DocID, materializedPath as Path from {Documents}
        WHERE dID NOT IN (SELECT dID FROM {contentOnDisk})"
                ;
        }

        #endregion
    }
}