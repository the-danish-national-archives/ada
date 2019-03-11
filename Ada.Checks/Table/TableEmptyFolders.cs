namespace Ada.Checks.Table
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class TableEmptyFolders : AdaAvDynamicFromSql
    {
        #region  Constructors

        protected TableEmptyFolders()
            : base("4.D_6")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT (relativePath || '\' || name) AS Path FROM tableFoldersOnDisk WHERE fileCount = 0"
                ;
        }

        #endregion
    }
}