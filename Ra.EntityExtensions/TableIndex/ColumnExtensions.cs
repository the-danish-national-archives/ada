namespace Ra.EntityExtensions.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using DomainEntities.TableIndex;

    #endregion

    public static class ColumnExtensions
    {
        #region

        public static IEnumerable<ForeignKey> GetForeignKeys(this Column column)
        {
            return
                column.ParentTable.ForeignKeys?.Where(
                    f => f.References?.Any(r => string.CompareOrdinal(r.Column, column.Name) == 0) ?? false);
        }

        public static string GetNormalizedType(this Column column)
        {
            var pos = column.Type.IndexOf('(');
            if (pos != -1) return column.Type.Substring(0, pos).Trim().ToUpper();

            return column.Type.Trim().ToUpper();
        }

        public static string GetXmlMappedType(this Column column)
        {
            var type = column.GetNormalizedType(); // trims and toUpper
            //from tableIndex.xsd (sorted as seen there)
            switch (type)
            {
//                < character string type>
                case "CHARACTER":
                case "CHAR":
                case "CHARACTER VARYING":
                case "CHAR VARYING":
                case "VARCHAR":
//                    < national character string type>
                case "NATIONAL CHARACTER":
                case "NATIONAL CHAR":
                case "NCHAR":
                case "NATIONAL CHARACTER VARYING":
                case "NATIONAL CHAR VARYING":
                case "NCHAR VARYING":
//                case "NVARCHAR": // not apearing?
                    return "string";
//                    < exact numeric type>
                case "NUMERIC":
                case "DECIMAL":
                case "DEC":
                    return "decimal";
                case "INTEGER":
                case "INT":
                case "SMALLINT":
//                case "SMALL INTEGER": // not apearing
                    return "integer";
//                    < approximate numeric type>
                case "FLOAT":
                    return "float";
                case "REAL":
                case "DOUBLE PRECISION":
                    return "double";
//                    < boolean type >
                case "BOOLEAN":
                    return "boolean";
//                    < datetime type >
                case "DATE":
                    return "date";
                case "TIME":
                case "TIMEWITH TIME ZONE":
                case "TIME WITH TIME ZONE":
                case "TIMEWITHOUT TIME ZONE":
                case "TIME WITHOUT TIME ZONE":
                    return "time";
                case "TIMESTAMP":
                case "TIMESTAMPWITH TIME ZONE":
                case "TIMESTAMP WITH TIME ZONE":
                case "TIMESTAMPWITHOUT TIME ZONE":
                case "TIMESTAMP WITHOUT TIME ZONE":
                    return "dateTime";
//                    < interval type >
                case "INTERVAL":
                    return "duration";
                default:
                    return type;
            }
        }

        public static bool IsForeignKey(this Column column)
        {
            return GetForeignKeys(column).Any();
        }

        public static bool IsPrimaryKey(this Column column)
        {
            return column.ParentTable.PrimaryKey.Columns.Any(c => string.CompareOrdinal(c, column.Name) == 0);
        }

        #endregion
    }
}