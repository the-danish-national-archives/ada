namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class TableIndexInvalidDescription : AdaAvDynamicFromSql
    {
        #region  Constructors

        public TableIndexInvalidDescription()
            : base("6.C_1")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT tableName as TableName, folder as FolderName, description as TableDescription FROM tables  WHERE length(description) < 3"
                ;
        }

        #endregion
    }
}