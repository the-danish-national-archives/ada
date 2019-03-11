namespace Ada.Test.UnitTests.AdaAvChecks.AdaAvDynamicFromSql
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using AutoRunTestsuite;

    #endregion

    public class QueryTestCase
    {
        #region  Constructors

        public QueryTestCase
        (
            //            Type type,
            string avidSa, LogEventTestCase logEventTestCase, int repeated = 1)
        {
            //            Type = type;
            AvidSa = avidSa;
            var list = new List<LogEventTestCase> {logEventTestCase};
            LogEventTestCases = list;

            if (repeated <= 1)
                return;
            while (--repeated != 0) list.Add(logEventTestCase);
        }

        public QueryTestCase
        (
            //            Type type,
            string avidSa, params LogEventTestCase[] cases)
        {
            //            Type = type;
            AvidSa = avidSa;
            LogEventTestCases = cases.AsEnumerable();
        }

        #endregion

        #region Properties

        public string AvidSa { get; }

        public IEnumerable<LogEventTestCase> LogEventTestCases { get; }

        #endregion

        #region

        public IEnumerable<LogEntrySimple> ToLogEntrySimpleList()
        {
            return LogEventTestCases.Select(c => c.ToLogEntrySimple());
        }

        #endregion
    }
}