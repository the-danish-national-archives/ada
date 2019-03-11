namespace Ada.Checks.Table
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    public class TableLimitsOfColumnsOfDateTime : AdaAvCheckNotification
    {
        #region  Constructors

        public TableLimitsOfColumnsOfDateTime(Column column, Tuple<string, string> minMax)
            : base("4.D_11")
        {
            TableName = column.TableName;
            ColumnName = column.Name;
            ColumnType = column.Type;
            MinValue = minMax.Item1;
            MaxValue = minMax.Item2;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string ColumnName { get; set; }

        [AdaAvCheckNotificationTag]
        public string ColumnType { get; set; }

        [AdaAvCheckNotificationTag]
        public string MaxValue { get; set; }

        [AdaAvCheckNotificationTag]
        public string MinValue { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table, TableContentRepo tableContentRepo)
        {
            foreach (var column in GetDateTimeColumns(table))
            {
                var minMax = tableContentRepo.GetMinMaxDateTimeValues(column);
                if (minMax != null) yield return new TableLimitsOfColumnsOfDateTime(column, minMax);
            }
        }

        private static IEnumerable<Column> GetDateTimeColumns(Table table)
        {
            return table.Columns.Select(x => x)
                .Where(x => x.Type == "DATE" || x.Type == "TIME" || x.Type == "TIMESTAMP");
        }

        #endregion
    }
}