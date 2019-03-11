namespace Ada.Checks.Table
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class TablesFolderSequenceGaps : AdaAvViolation
    {
        #region  Constructors

        public TablesFolderSequenceGaps(IEnumerable<string> missingTables)
            : base("4.D_3")
        {
            MissingTables = missingTables;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTagSmartToString]
        public IEnumerable<string> MissingTables { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(TableIndex index)
        {
            var gaps = GetFolderSequenceGaps(index).ToList();
            if (gaps.Any()) yield return new TablesFolderSequenceGaps(gaps);
        }

        private static List<string> GetFolderSequenceGaps(TableIndex index)
        {
            var sequence = index.Tables.Where(x => x.FolderNameIsValid()).Select(x => int.Parse(x.Folder.OnlyTrailingDigits())).ToList();
            var result = new List<string>();

            if (sequence.Any())
                result =
                    Enumerable.Range(sequence.Min(), sequence.Count).Except(sequence).Select(
                        x => "table" + x.ToString()).ToList();
            return result;
        }

        #endregion
    }
}