namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;

    #endregion

    public class TableTestNumberingGaps : AdaAvViolation
    {
        #region  Constructors

        public TableTestNumberingGaps(IEnumerable<string> missingTables)
            : base("4.D_5")
        {
            MissingTables = missingTables;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTagSmartToString]
        public IEnumerable<string> MissingTables { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(List<string> tablesOnDisk)
        {
            var sequence = tablesOnDisk.Select(x => int.Parse(x.Replace("table", string.Empty))).ToList();
            var gaps =
                Enumerable.Range(sequence.Min(), sequence.Count).Except(sequence).Select(x => "table" + x.ToString())
                    .ToList();

            if (gaps.Any()) yield return new TableTestNumberingGaps(gaps);
        }

        #endregion
    }
}