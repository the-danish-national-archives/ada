namespace Ada.Test.UnitTests.AdaAvChecks
{
    #region Namespace Using

    using System;
    using ChecksBase;
    using Log.Entities;
    using NUnit.Framework;

    #endregion

    internal class AdaAvCheckToAvQueryAttributeTest
    {
        #region  Constructors

        public AdaAvCheckToAvQueryAttributeTest()
        {
            First = 13;
        }

        #endregion

        #region Properties

        public int First { get; set; }

        public string[] Parameters { get; set; }

        #endregion

        #region

        [AdaAvCheckToAvQuery]
        public static string ConverterMethod(int first)
        {
            return (first + 5).ToString();
        }

        [Test]
        public void Tester()
        {
            var logEntry = new LogEntry();
            logEntry.AddTag(nameof(First), First.ToString());

            var method = typeof(AdaAvCheckToAvQueryAttributeTest).GetMethod(nameof(ConverterMethod));

            var subject = new AdaAvCheckToAvQueryAttribute(nameof(First));

            var res = subject.CreateFromLogEntryToAvQueryConverter(method, logEntry);

            var convertedType = res as Func<Func<string, int>, string>;
            Assert.IsNotNull(convertedType);

            var convertedValue = convertedType(s => int.Parse(s));
            Assert.AreEqual(ConverterMethod(First), convertedValue);
        }

        #endregion
    }
}