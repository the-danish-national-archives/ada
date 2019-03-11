namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using AutoRunTestsuite;

    #endregion

    public class LogEventTestCase
    {
        #region Properties

        public Dictionary<string, string> Tags { get; set; }
        public string Type { get; set; }

        #endregion

        #region

        public LogEntrySimple ToLogEntrySimple()
        {
            if (Tags == null) return new LogEntrySimple {EntryTypeId = Type, Tags = new List<LogEntrySimpleTag>()};
            return new LogEntrySimple
            {
                EntryTypeId = Type,
                Tags =
                    Tags.Select(p => new LogEntrySimpleTag {Type = p.Key, Text = p.Value})
                        .ToList()
            };
        }

        #endregion

        //public static LogEventTestCase Empty = new LogEventTestCase();
    }
}