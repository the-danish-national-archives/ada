#region Header

// Author 
// Created 19

#endregion

namespace Ada.Test.UnitTests.AdaAvChecks
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using AdaAvDynamicFromSql;
    using AutoRunTestsuite;
    using ChecksBase;
    using Common;
    using Log;
    using Log.Entities;
    using NUnit.Framework;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    public abstract class AgainstPreLoadedDatabaseTest<T>
    {
        #region  Fields

        protected DirectoryInfo DatabasePath;
        protected AViD Id;

        #endregion

        #region

        protected abstract void Act
            (AdaProcessLog adaProcessLog, AdaTestLog adaTestLog, AdaTestUowFactory adaTestUowFactory, AVMapping mapping);

        protected void CollectiveTest
        (
            string inputAvid,
            IEnumerable<LogEntrySimple> inputXml)
        {
            // Arrange
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            Id = new AViD(inputAvid);
            var mapping = AVMapping.CreateMapping(Id, new List<string>());

            var logDBFolder = new DirectoryInfo($@".\tests\{typeof(T).FullName}\{inputAvid}");

            if (!logDBFolder.Exists)
            {
                logDBFolder.Create();
                logDBFolder.Refresh();
            }

            var logUowFactory = new AdaLogUowFactory(Id, "log", null);

            logUowFactory.DeleteDataBase();
            logUowFactory.CreateDataBase();
            var errorTexts =
                new FileInfo(
                    Path.Combine(
                        @".\",
                        "errortexts.xml"));

            AdaTestLog.LoadErrorTypesFromFile(errorTexts, logUowFactory);

            var adaProcessLog = new AdaProcessLog(logUowFactory);

            var testLog = new AdaTestLog(logUowFactory, Guid.NewGuid(), GetType(), 1);

            DatabasePath = new DirectoryInfo(@".\databases");

            var adaTestUowFactory = new AdaTestUowFactory(Id, "test", DatabasePath);

            var eventsExpected = inputXml?.ToList() ?? new List<LogEntrySimple>();

            Act(adaProcessLog, testLog, adaTestUowFactory, mapping);

            // assert
            using (var unitOfWork = logUowFactory.GetUnitOfWork())
            {
                unitOfWork.BeginTransaction();

                var eventRepo = unitOfWork.GetRepository<LogEntry>();

                var eventsActual = eventRepo.All().Select(entry => new LogEntrySimple(entry)).ToList();
                //eventsActual.Sort();

#if DEBUG
                if (eventsActual.Any())
                    foreach (var logEntrySimple in eventsActual)
                    {
                        var sb = new StringBuilder();

                        sb.Append(
                            @"new LogEventTestCase
                    {
                        Type = """);
                        sb.Append(
                            logEntrySimple.EntryTypeId);
                        sb.Append(
                            @""",
                                    ");

                        sb.AppendLine(@"Tags = new Dictionary<string, string>()
                                                   {");
                        foreach (var tag in logEntrySimple.Tags)
                        {
                            sb.Append(@"        { """);
                            sb.Append(tag.Type);
                            sb.Append(@""", @""");
                            sb.Append(tag.Text);
                            sb.AppendLine(@""" }, ");
                        }

                        sb.Append(@"
                                                   }
                    },");
                        Console.WriteLine(sb.ToString().Substring(0, sb.Length - 2));
                    }

#endif

                Assert.AreEqual(eventsExpected.Count, eventsActual.Count, "Number of events differ");

                Assert.AreEqual(eventsExpected, eventsActual);

                // Make sure all tags have been replaced from errortexts.xml
                foreach (var entrySimple in eventsActual)
                {
                    Assert.False(
                        entrySimple.FormattedText.Contains('{'),
                        $"Bad format text (contains '{{'): {entrySimple.FormattedText}");
                    Assert.False(
                        entrySimple.FormattedText.Contains('}'),
                        $"Bad format text (contains '}}'): {entrySimple.FormattedText}");
                }
            }
        }

        protected static void ReportAny(AdaTestLog adaTestLog, IEnumerable<AdaAvCheckNotification> notifications)
        {
            foreach (var logEntry in notifications.Select(n => n.ToLogEntry())) adaTestLog.LogError(logEntry);
        }

        #endregion

        #region Nested type: TestCaseSource

        public abstract class TestCaseSource
        {
            #region Properties

            public abstract string Catagory { get; }

            public abstract List<QueryTestCase> Tests { get; }

            #endregion
        }

        #endregion

        #region Nested type: TestExtractor

        protected class TestExtractor
        {
            #region

            public static IEnumerable<TestCaseData> TestCases(Type type)
            {
                var data = Activator.CreateInstance(type) as TestCaseSource;

                if (data == null)
                    throw new ArgumentNullException();

                return
                    data.Tests.Select(
                        t =>
                            new TestCaseData(
                                    t.AvidSa,
                                    t.ToLogEntrySimpleList()).SetName($"{typeof(T).Name}({t.AvidSa})")
                                .SetCategory(data.Catagory));
            }

            #endregion
        }

        #endregion
    }
}