namespace Ada.Core
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using ActionBase;
    using Actions;
    using Actions.IngestActions;
    using Actions.PresenterAction;
    using Actions.Rewrite;
    using Actions.TestActions;
    using Common;
    using Log;
    using Properties;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    public static class Controller
    {
        #region TiffChecksCopy enum

        [Flags]
        public enum TiffChecksCopy
        {
            NONE = 0,

            COMPRESSION = 1 << 0,

            PRIVATE_TAGS = 1 << 1,

            BLANK_FIRST_PAGE = 1 << 2,

            ALL_PAGES_BLANK = 1 << 3,

            ALL = COMPRESSION | PRIVATE_TAGS | BLANK_FIRST_PAGE | ALL_PAGES_BLANK
        }

        #endregion

        #region Static

        public static TestSettings settings;

        private static AViD ID;

        private static AVMapping Mapping;

//        public static void RewriteFileIndex()
//        {
//            if (Mapping == null || ID == null)
//                return;
//
//
//            ActionLoop(
//                AllExecutionOrders.Where(o => o.Action == typeof(FileIndexRewriteAction)));
//        }
//
//        public static void RewriteXsd()
//        {
//            if (Mapping == null || ID == null)
//                return;
//
//            ActionLoop(
//                AllExecutionOrders.Where(o => o.Action == typeof(XsdRewriteAction)));
//        }


        public static readonly List<ExecutionOrder> ExecutionOrders =
            new List<ExecutionOrder>
            {
                new ExecutionOrder(
                    typeof(StructureTestReadFilePresenterAction),
                    nameof(AVStatus.StructureReadFiles), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(StructureTestFileCheckPresenterAction),
                    nameof(AVStatus.StructureCheckFiles), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(Md5IngestAction),
                    nameof(AVStatus.CalculateMD5), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(ReadFileIndexTestPresenterAction),
                    nameof(AVStatus.ReadFileIndex), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(StructureTestMd5CheckPresenterAction),
                    nameof(AVStatus.CheckMD5), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(ArchiveIndexTestPresenterAction),
                    nameof(AVStatus.ArchiveInformation), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(DocumentationIndexTestPresenterAction),
                    nameof(AVStatus.ContextDocumentationIndex), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(ContextDocumentTestPresenterAction),
                    nameof(AVStatus.ContextDocumentation), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(TableIndexIngestPresenterAction),
                    nameof(AVStatus.TableIndex), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(TableIngestPresenterAction),
                    nameof(AVStatus.Tables), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(PrimaryKeyTestAction),
                    nameof(AVStatus.PrimaryKeys), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(ForeignKeyTestAction),
                    nameof(AVStatus.ForeignKeys), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(ViewTestAction),
                    nameof(AVStatus.Views), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(FunctionalDescriptionTestAction),
                    nameof(AVStatus.FunctionalDescriptions), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(DocIndexTestPresenterAction),
                    nameof(AVStatus.DocumentIndex), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(DocumentTestPresenterAction),
                    nameof(AVStatus.Documents), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(DocumentRelationTestAction),
                    nameof(AVStatus.DocRelationTest), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(DummyLastOrder),
                    "", "")
            };

        public static readonly IEnumerable<ExecutionOrder> AllExecutionOrders =
            ExecutionOrders.Union(new List<ExecutionOrder>
            {
                new ExecutionOrder(
                    typeof(XsdRewriteAction),
                    nameof(AVStatus.RewriteXsd), typeof(AVStatus)),
                new ExecutionOrder(
                    typeof(FileIndexRewriteAction),
                    nameof(AVStatus.WriteFileIndex), typeof(AVStatus))
            });

        #endregion

        #region

        private static void ActionLoop(IEnumerable<ExecutionOrder> orders)
        {
            var dbFolder = Settings.Default.DBCreationFolder;
            using (var adaTestUowFactory = new AdaTestUowFactory(ID, "test", new DirectoryInfo(dbFolder)))
            using (var adaAvUowFactory = new AdaAvUowFactory(ID, "av", new DirectoryInfo(dbFolder)))
            using (var adaModelUowFactory = new AdaAvUowFactory(ID, "model", new DirectoryInfo(dbFolder)))
            using (var adaLogUowFactory = new AdaLogUowFactory(ID, "log", new DirectoryInfo(dbFolder)))
            {
                foreach (var next in orders)
                {
                    Debug.WriteLine($"Moving along to {next.Action}.");

                    RunAction(next, dbFolder, adaTestUowFactory, adaAvUowFactory, adaModelUowFactory,
                        adaLogUowFactory);
                }
            }
        }

        public static void DoFullTest(AViD avid, TestSettings testSettings)
        {
            ID = avid;
            settings = testSettings;

            Mapping = AVMapping.CreateMapping(
                avid,
                testSettings.Drives.GetActiveDrives().Union(testSettings.InputDictionaries).ToList());


//            ActionLoop(ExecutionOrders);
            ActionLoop(AllExecutionOrders);
        }


        public static Type GetTypeOfLastCommand()
        {
            return ExecutionOrders.Last().Action;
        }

        private static bool HasDocuments(AdaTestUowFactory testFactory)
        {
            using (var repo = new AdaStructureRepo(testFactory, 0))
            {
                return repo.HasDocuments();
            }
        }

        public static void Initialize()
        {
        }

        public static event LoggingEventHandler LoggingEvent;

        private static void LogResult(ActionRunResult? res, ExecutionOrder order)
        {
            LoggingEventType loggingEvent;
            var text = order.VisualName ?? "";

            switch (res)
            {
                case ActionRunResult.Error:
                    // exceptional extra event on error from ActionResult
                    OnLoggingEvent(new LoggingEventArgs(order.Action, LoggingEventType.Error,
                        order.VisualName == null ? "" : text));
                    loggingEvent = LoggingEventType.TestEnd;
                    text += " fædig med fejl";
                    break;
                case ActionRunResult.TestSkipped:
                    loggingEvent = LoggingEventType.TestSkipped;
                    text += " springes over: " + DateTime.Now;
                    break;
                case ActionRunResult.TestSkippedPreConditionsNotMet:
                    loggingEvent = LoggingEventType.TestSkippedPreConditionsNotMet;
                    text += " springes over: " + DateTime.Now;
                    break;
                case ActionRunResult.FastRun:
                    loggingEvent = LoggingEventType.FastRun;
                    text += " springes over: " + DateTime.Now;
                    break;
                case ActionRunResult.Done:
                    loggingEvent = LoggingEventType.TestEnd;
                    text += " færdig " + DateTime.Now;
                    break;
                default:
                    loggingEvent = LoggingEventType.TestSkipped;
                    text += " springes over: " + DateTime.Now;
                    break;
            }

            text += "\n";

            OnLoggingEvent(new LoggingEventArgs(order.Action, loggingEvent, order.VisualName == null ? "" : text));
        }

        private static void LogStart(ExecutionOrder order)
        {
            OnLoggingEvent(
                new LoggingEventArgs(order.Action, LoggingEventType.TestStart,
                    order.VisualName == null ? "" : "Starter " + order.VisualName + DateTime.Now
                ));
        }


        private static bool MarkedWithDocuments(AdaTestUowFactory testFactory)
        {
            using (var repo = new AdaStructureRepo(testFactory, 0))
            {
                return repo.MarkedWithDocuments();
            }
        }

        private static void OnLoggingEvent(LoggingEventArgs loggingevent)
        {
            LoggingEvent?.Invoke(null, loggingevent);
        }

        private static void ProgressUpdate(ExecutionOrder order, string text)
        {
            OnLoggingEvent(
                new LoggingEventArgs(order.Action, LoggingEventType.ProgressUpdate,
                    text
                ));
        }

        private static void RunAction
        (
            ExecutionOrder order,
            string dbFolder,
            AdaTestUowFactory testFactory,
            AdaAvUowFactory avFactory,
            AdaAvUowFactory modelFactory,
            AdaLogUowFactory logUowFactory)
        {
            var log = new AdaTestLog(logUowFactory, Guid.NewGuid(), order.Action, 1);
            var processLog = new AdaProcessLog(logUowFactory);

            LogStart(order);
            ActionRunResult? res = null;

            try
            {
                // Expected to be replaced by logic using xml-configuration of some kind
                // Also might be dependent on targets later on?
                switch (order.Action.Name)
                {
                    case "ArchiveIndexTestPresenterAction":
                        if (settings.TableTest.Active)
                            res = new ArchiveIndexTestPresenterAction(processLog, log, Mapping).Run(testFactory);
                        break;
                    case "DocIndexTestPresenterAction":
                        if (settings.Documents.Active)
                        {
                            if (HasDocuments(testFactory))
                                res = new DocIndexTestPresenterAction(processLog, log, Mapping).Run(testFactory);
                            else
                                res = ActionRunResult.TestSkippedPreConditionsNotMet;
                        }

                        break;
                    case "DocumentationIndexTestPresenterAction":
                        if (settings.Documentation.Active)
                            res = new DocumentationIndexTestPresenterAction(processLog, log, Mapping).Run(testFactory);
                        break;
                    case "ContextDocumentTestPresenterAction":
                        if (settings.Documentation.Active)
                        {
                            var contextDocumentTestPresenterAction = new ContextDocumentTestPresenterAction(
                                processLog,
                                log,
                                Mapping,
                                (int) TiffChecks(settings.Documentation),
                                settings.Documentation.PageCount_High
                                    ? (int?) settings.Documentation.PageCount_Alert_Level
                                    : null,
                                dbFolder);

                            contextDocumentTestPresenterAction.ProgressCallback = s => ProgressUpdate(order, s);
                            res =
                                contextDocumentTestPresenterAction.Run(new AdaUowTarget(testFactory));
                        }

                        break;
                    case "DocumentRelationTestAction":
                        if (settings.Documents.Active && settings.TableTest.Active)
                        {
                            if (MarkedWithDocuments(testFactory))
                                using (var repo = new FullDocumentRepo(testFactory, Mapping.AVID, dbFolder))
                                {
                                    res = new DocumentRelationTestAction(processLog, log, Mapping).Run(repo);
                                }
                            else
                                res = ActionRunResult.TestSkippedPreConditionsNotMet;
                        }

                        break;
                    case "DocumentTestPresenterAction":
                        if (settings.Documents.Active)
                        {
                            if (HasDocuments(testFactory))
                            {
                                var documentTestPresenterAction = new DocumentTestPresenterAction(
                                    processLog,
                                    log,
                                    Mapping,
                                    (int) TiffChecks(settings.Documents),
                                    settings.Documents.PageCount_High
                                        ? (int?) settings.Documents.PageCount_Alert_Level
                                        : null,
                                    dbFolder);
                                documentTestPresenterAction.ProgressCallback = s => ProgressUpdate(order, s);
                                res =
                                    documentTestPresenterAction.Run(new AdaUowTarget(testFactory));
                            }
                            else
                            {
                                res = ActionRunResult.TestSkippedPreConditionsNotMet;
                            }
                        }

                        break;
                    case "ForeignKeyTestAction":
                        if (settings.TableTest.Active && settings.TableTest.ForeignKeyTest)
                            res = new ForeignKeyTestAction(processLog, log, Mapping, avFactory).Run(testFactory);
                        break;
                    case "FunctionalDescriptionTestAction":
                        if (settings.TableTest.Active)
                            res = new FunctionalDescriptionTestAction(processLog, log, Mapping).Run(testFactory);
                        break;
                    case "Md5IngestAction":
                        if (settings.FileIndex.MD5Test)
                        {
                            var md5IngestAction = new Md5IngestAction(processLog, log, Mapping);

                            md5IngestAction.ProgressCallback = s => ProgressUpdate(order, s);

                            res = md5IngestAction.Run(testFactory);
                        }

                        break;
                    case "PrimaryKeyTestAction":
                        if (settings.TableTest.Active && settings.TableTest.PrimaryKeyTest)
                            res = new PrimaryKeyTestAction(processLog, log, Mapping, avFactory).Run(testFactory);
                        break;
                    case "ReadFileIndexTestPresenterAction":
                        if (true)
                            res =
                                new ReadFileIndexTestPresenterAction(processLog, log, Mapping).Run(
                                    new AdaUowTarget(testFactory));
                        break;
                    case "FileIndexRewriteAction":
                        if (true)
                        {
                            var path = string.Concat(
                                Mapping.GetMediaRoot(1),
                                Mapping.AVID.FullID,
                                ".1\\Indices\\",
                                "fileIndex",
                                ".xml");
                            res = new FileIndexRewriteAction(processLog, log, Mapping, testFactory).ResetAndRun(path);
                        }

                        break;
                    case "XsdRewriteAction":
                        if (settings.TableTest.Active)
                            res = new XsdRewriteAction(processLog, log, Mapping).ResetAndRun(testFactory);
                        break;
                    case "StructureTestFileCheckPresenterAction":
                        res =
                            new StructureTestFileCheckPresenterAction(processLog, log, Mapping, dbFolder).Run(
                                new AdaUowTarget(testFactory));
                        break;
                    case "StructureTestMd5CheckPresenterAction":
                        res =
                            new StructureTestMd5CheckPresenterAction(processLog, log, Mapping).Run(
                                new AdaUowTarget(testFactory));
                        break;
                    case "StructureTestReadFilePresenterAction":
                        res =
                            new StructureTestReadFilePresenterAction(processLog, log, Mapping).Run(
                                new AdaUowTarget(testFactory));
                        break;
                    case "TableIndexIngestPresenterAction":
                        if (settings.TableTest.Active)
                            res = new TableIndexIngestPresenterAction(processLog, log, Mapping, avFactory, modelFactory)
                                .Run(testFactory);
                        break;
                    case "AdaInternalAction":
                        if (settings.TableTest.Active)
                            res = ActionRunResult.Done;
                        break;
                    case "TableIngestPresenterAction":
                        if (settings.TableTest.Active)
                        {
                            var tableIngestPresenterAction =
                                new TableIngestPresenterAction(processLog, log, Mapping, avFactory);

                            tableIngestPresenterAction.ProgressCallback = s => ProgressUpdate(order, s);
                            res =
                                tableIngestPresenterAction.Run(
                                    new AdaUowTarget(testFactory));
                        }

                        break;
                    case "ViewTestAction":
                        if (settings.TableTest.Active)
                            res = new ViewTestAction(processLog, log, Mapping, avFactory).Run(testFactory);
                        break;
                }
            }
            catch (AdaSkipAllActionException)
            {
                res = ActionRunResult.Done;
            }

            log.Flush();

            LogResult(res, order);
        }

        private static TiffChecksCopy TiffChecks(DocumentTestSettings testSettings)
        {
            var checks = TiffChecksCopy.NONE;
            if (testSettings.Private_Tags)
                checks |= TiffChecksCopy.PRIVATE_TAGS;
            if (testSettings.Compression)
                checks |= TiffChecksCopy.COMPRESSION;
            if (testSettings.Blank_First_Page)
                checks |= TiffChecksCopy.BLANK_FIRST_PAGE;
            if (testSettings.Blank_All_Documents)
                checks |= TiffChecksCopy.ALL_PAGES_BLANK;
            return checks;
        }

        private static TiffChecksCopy TiffChecks(DocumentationTestSettings testSettings)
        {
            var checks = TiffChecksCopy.NONE;
            if (testSettings.Private_Tags)
                checks |= TiffChecksCopy.PRIVATE_TAGS;
            if (testSettings.Compression)
                checks |= TiffChecksCopy.COMPRESSION;
            if (testSettings.Blank_First_Page)
                checks |= TiffChecksCopy.BLANK_FIRST_PAGE;
            if (testSettings.Blank_All_Documents)
                checks |= TiffChecksCopy.ALL_PAGES_BLANK;
            return checks;
        }

        #endregion
    }

    internal class DummyLastOrder : IAdaAction
    {
    }
}