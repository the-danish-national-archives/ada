namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexSequenceGaps : AdaAvViolation
    {
        #region  Constructors

        public TableIndexSequenceGaps(Table table)
            : base("6.C_4")
        {
            TableName = table.Name;
            FolderNumber = table.Folder;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string FolderNumber { get; set; }

        [AdaAvCheckNotificationTag]
        public string TableName { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(Table table)
        {
            if (SequenceGaps(table).Any()) yield return new TableIndexSequenceGaps(table);
        }

        public static IEnumerable<string> SequenceGaps(Table table)
        {
            var sequence = table.Columns.Select(x => int.Parse(x.ColumnId.ToLower().Replace("c", ""))).ToList();
            return Enumerable.Range(sequence.Min(), sequence.Count).Except(sequence).Select(x => x.ToString());
        }

        #endregion
    }
}