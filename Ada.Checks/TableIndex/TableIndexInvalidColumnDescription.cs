namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System;
    using ChecksBase;

    #endregion

    public class TableIndexInvalidColumnDescription : AdaAvDynamicFromSql
    {
        #region  Constructors

        public TableIndexInvalidColumnDescription()
            : base("6.C_5")
        {
        }

        #endregion

        #region

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT colName as ColumnName, colNumber as ColumnNumber, colDescription as ColumnDescription, tableName as TableName FROM columns  WHERE length(colDescription) < 3"
                ;
        }

        #endregion
    }
}