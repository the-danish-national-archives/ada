namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableContentRepo : RepoBase
    {
        #region  Constructors

        public TableContentRepo(IAdaUowFactory uowFactory, int commitInterval) : base(uowFactory, commitInterval)
        {
        }

        #endregion

        #region

        public void AddRow(string insertTemplate, SortedList<string, string> rowContents)
        {
            var commandText = string.Format(insertTemplate, rowContents.Values.SmartToString(",", true));
            ExecuteNonQuery(commandText);
        }

        public static string GetInsertTemplate(Table table)
        {
            var tableName = table.Name;
            var columns = table.Columns.OrderBy(x => x.ColumnId).Select(x => x.Name).SmartToString();

            var sqlInsertTemplate = $"INSERT INTO {tableName}({columns})";

            return sqlInsertTemplate + " VALUES({0});";
        }

        public
            Tuple<string, string>
//            Tuple<DateTime,DateTime>
            GetMinMaxDateTimeValues(Column column)
        {
            const string minMaxQueryTemplate = "SELECT MIN({ColumnName}), MAX({ColumnName}) FROM {TableName}";
            var formatParams = new {ColumnName = column.Name, column.TableName};


            return EnumerateQuery(minMaxQueryTemplate.FormatWith(formatParams))
                .Where(reader => !reader.IsDBNull(0) && !reader.IsDBNull(1))
//                .Where(reader => reader.GetDataTypeName(0) == "DateTime" && reader.GetDataTypeName(1) == "DateTime")
//                .Select(reader => new Tuple<DateTime,DateTime>(reader.GetDateTime(0),reader.GetDateTime(1)))
                .Select(reader => new Tuple<string, string>(reader.GetString(0), reader.GetString(1)))
                .ToList() // Hack - To list to make sure enerumeration is finished
                .FirstOrDefault();

//            using (var cmd = this.Connection.CreateCommand())
//            {
//                cmd.CommandText = minMaxQueryTemplate.FormatWith(formatParams);
//                var reader = cmd.ExecuteReader();
//                if (reader.Read() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
//                {
//                    return ;
//                }
//            }
//            return null;
        }

        public void UpdateSchema(string createSql)
        {
            //const string commandText = "PRAGMA writable_schema = 1; delete from sqlite_master where type in ('table', 'index', 'trigger'); PRAGMA writable_schema = 0; VACUUM; PRAGMA INTEGRITY_CHECK;";
            //this.ExecuteNonQuery(commandText);
            ExecuteNonQuery(createSql);
        }

        #endregion
    }
}