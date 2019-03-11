namespace Ada.Checks.Documents
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public abstract class XBadFileTypes : AdaAvDynamicFromSql
    {
        #region  Fields

        private readonly XToDiskContent _toDiskContent;

        #endregion

        #region  Constructors

        protected XBadFileTypes(string tagType, XToDiskContent toDiskContent)
            : base(tagType)
        {
            _toDiskContent = toDiskContent;
        }

        #endregion

        #region

        protected sealed override string GetTestQuery(Type type)
        {
            return
                $@"SELECT relativePath as Path, name AS FileName, extension as Extension from {_toDiskContent.ContentOnDisk} 
                        WHERE LOWER(extension) NOT IN (SELECT LOWER(extension) FROM documentTypes)"
                ;
        }

        #endregion
    }
}