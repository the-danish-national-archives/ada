namespace Ada.Action64.IngestActions
{
    #region Namespace Using

    using System;
    using System.Linq;
    using System.Reflection;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.ContextDocIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.DocumentsContent;
    using Checks.FileIndex;
    using ChecksBase;
    using Common;
    using log4net;
    using Log;
    using Log.Entities;
    using Ra.Common;
    using Ra.DocumentInvestigator.AdaAvChecking.AdaReporting;
    using Ra.DocumentInvestigator.AdaAvChecking.AudioVideo;
    using Ra.DocumentInvestigator.AdaAvChecking.Image;
    using Ra.DocumentInvestigator.AdaAvChecking.Image.Jp2;
    using Ra.DocumentInvestigator.AdaAvChecking.Image.Tiff;
    using Ra.DomainEntities;
    using Ra.DomainEntities.Documents;
    using Ra.EntityExtensions.FileIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("docIndex", "files")]
    [RequiredChecks(
        typeof(FolderStructureFirstMediaMissing),
        typeof(FolderStructureMediaNumberGaps),
        typeof(FolderStructureMissingIndicesFirstMedia),
        typeof(FolderStructureSchemasMissing),
        typeof(FolderStructureTablesMissing),
        typeof(IndicesFileIndex),
        typeof(IndicesArchiveIndex),
        typeof(SchemaMissingFolder),
        typeof(DiskSpaceWarning),
        typeof(FolderStructureDuplicateMediaNumber),
        typeof(FileIndexNotWellFormed),
        typeof(FileIndexInvalid),
        typeof(AdaAvSchemaVersionFileIndex),
        typeof(AdaAvSchemaVersionXmlSchema),
        typeof(ContextDocumentationInvalid),
        typeof(ContextDocMissingDoc),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed)
    )]
    [ReportsChecks(
        typeof(DocumentsTiffStandard),
        typeof(DocumentsTiffCompression),
        typeof(DocumentsTiffOddBitDepth),
        typeof(DocumentsTiffPrivateTags),
        typeof(DocumentsTiffBlankFirstPages),
        typeof(DocumentsTiffBlankPages),
        typeof(DocumentsBadContent),
        typeof(DocumentsHighPageCount),
        typeof(DocumentsMP3Standard),
        typeof(DocumentsWaveStandard),
        typeof(DocumentsTiffUnreadablePages),
        typeof(DocumentsVideoFormat),
        typeof(DocumentsVideoContent)
    )]
    public class DocumentsOtherIngestAction : AdaActionBase<FullDocumentRepoHacker>
    {
        #region  Fields

        private readonly int? _highPageCount;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _checks;

        #endregion

        #region  Constructors

        public DocumentsOtherIngestAction
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, int checks, int? highPageCount)
            : base(processLog, testLog, mapping)
        {
            _checks = checks;
            _highPageCount = highPageCount;
        }

        #endregion

        #region

        private AdaAvCheckNotification DocLogEntryToDocumentCheck
            (DocLogEntry tiffLogEvent, DocIndexEntry docInfo, string path)
        {
            //ignoring normal switch-case if(justpassedthreshold)
            switch (tiffLogEvent.EventTypeId)
            {
                case "5.E_2":
                    if (BaseDocObject.TresholdPassed)
                    {
                        if (BaseDocObject.ThirdTresholdPassed && !BaseDocObject.JustPassedTreshold)
                        {
                            tiffLogEvent.EventTypeId = "5.E_14";
                            path = "Dette dokument er sprunget over grundet for mange hukommelsesfejl med tidligere dokumenter!";
                            return new DocumentsTiffUnreadablePages(docInfo, path);
                        }

                        if (BaseDocObject.JustPassedTreshold)
                        {
                            tiffLogEvent.EventTypeId = "5.E_14";
                            BaseDocObject.JustPassedTreshold = false;
                            return new DocumentsTiffUnreadablePages(docInfo, path);
                        }
                    }
                    return new DocumentsTiffStandard(docInfo, path);
                case "5.E_3":
                    var comp = tiffLogEvent.EventTags.FirstOrDefault(t => t.TagType == "CompressionType").TagText
                               ?? string.Empty;
                    return new DocumentsTiffCompression(docInfo, path, comp);
                case "5.E_5":
                    return new DocumentsTiffOddBitDepth(docInfo, path);
//                case "5.E_7":
//                    return new DocumentsTiffPrivateTags(docInfo, path);
                case "5.E_10":
                    return new DocumentsTiffBlankFirstPages(docInfo, path);
                case "5.E_11":
                    if (BaseDocObject.TresholdPassed)
                    {
                        if (BaseDocObject.ThirdTresholdPassed && !BaseDocObject.JustPassedTreshold)
                        {
                            tiffLogEvent.EventTypeId = "5.E_14";
                            path = "Dette dokument er sprunget over grundet for mange hukommelsesfejl med tidligere dokumenter!";
                            return new DocumentsTiffUnreadablePages(docInfo, path);
                        }

                        if (BaseDocObject.JustPassedTreshold)
                        {
                            tiffLogEvent.EventTypeId = "5.E_14";
                            BaseDocObject.JustPassedTreshold = false;
                            return new DocumentsTiffUnreadablePages(docInfo, path);
                        }
                    }

                    return new DocumentsTiffBlankPages(docInfo, path);
                case "5.E_12":
                    return new DocumentsBadContent(docInfo, path);
                case "5.E_13":
                    var pageCount = tiffLogEvent.EventTags.FirstOrDefault(t => t.TagType == "PageCount").TagText
                                    ?? string.Empty;
                    return new DocumentsHighPageCount(docInfo, path, pageCount);
                case "5.E_14":
                    return new DocumentsTiffUnreadablePages(docInfo, path);
                case "5.F_1":
                    return new DocumentsMP3Standard(docInfo, path);
                case "5.F_2":
                    return new DocumentsWaveStandard(docInfo, path);
                case "5.F_3":
                    return new DocumentsVideoFormat(docInfo, path);
                case "5.F_4":
                    return new DocumentsVideoContent(docInfo, path);
                case "0.2":
                default:
                    return
                        new AdaAvInternalError(
                            new InvalidOperationException($"Check for DocLogEntry (EventTypeId = {tiffLogEvent.EventTypeId}"),
                            GetType());
            }
        }

        private void IngestBasic
            (FullDocumentRepoHacker documentIndexRepo, FileTypeEnum type, Action<BufferedProgressStream, DocInfo, Action<DocLogEntry>> testObject, Action reporter)
        {
            var files = documentIndexRepo.EnumerateDocFiles(
                type);
            var documentsTiffPrivateTagsSummary = new DocumentsTiffPrivateTags.DocumentsTiffPrivateTagsSummary(documentIndexRepo.IsForContextDocmunetation ? "ContextDocumentation" : "Documents");

            foreach (var entry in files)
            {
                reporter();

                var docEntry = entry.Item1;
                var fileEntry = entry.Item2;

                using (var bufferedProgressStream = fileEntry.GetAsStream(Mapping))
                {
                    if (bufferedProgressStream == null)
                        continue;

                    var di = new DocInfo
                    {
                        DocumentId = docEntry.DocumentId,
                        DocumentFolder = docEntry.DocumentFolder,
                        MediaId = docEntry.MediaId,
                        FileType = docEntry.SubmissionFileType
                    };

                    try
                    {
                        //Report(new AdaAvInternalError(new Exception("test"), GetType()));
                        testObject(
                            bufferedProgressStream,
                            di,
                            tiffLogEvent =>
                            {
                                if (tiffLogEvent.EventTypeId == "5.E_7")
                                    documentsTiffPrivateTagsSummary.Check(docEntry);
                                else
                                    Report(
                                        DocLogEntryToDocumentCheck(
                                            tiffLogEvent,
                                            docEntry,
                                            fileEntry.RelativePathAndFile()));
                            });
                    }
                    catch (Exception e)
                    {
                        log.Error($"File '{bufferedProgressStream.file.FullName}' caused an error! ", e);
                        if (BaseDocObject.TresholdPassed)
                        {
                            if (BaseDocObject.ThirdTresholdPassed && !BaseDocObject.JustPassedTreshold)
                            {
                                var newLogEntry = new LogEntry() { EntryTypeId = "5.E_14" };
                                newLogEntry.AddTag("Exception", $"[{newLogEntry.EntryTypeId}] not found in errortexts.xml");
                                newLogEntry.AddTag("Module", ToString());
                                LogEntryType entryType = null;
                                entryType=  AdaTestLog._errorTypes["5.E_14"];
                                newLogEntry.FormattedText = entryType.EntryText;
                                string path = "Dette dokument er sprunget over grundet for mange hukommelsesfejl med tidligere dokumenter!";
                                Report(new DocumentsTiffUnreadablePages(docEntry, path));
                                break;
                            }
                    
                            if (BaseDocObject.JustPassedTreshold)
                            {
                                var newLogEntry = new LogEntry() { EntryTypeId = "5.E_14" };
                                newLogEntry.AddTag("Exception", $"[{newLogEntry.EntryTypeId}] not found in errortexts.xml");
                                newLogEntry.AddTag("Module", ToString());
                                LogEntryType entryType = null;
                                entryType=  AdaTestLog._errorTypes["5.E_14"];
                                newLogEntry.FormattedText = entryType.EntryText;
                                //path = "Dette dokument er sprunget over grundet for mange hukommelsesfejl med tidligere dokumenter!";
                                Report(new DocumentsTiffUnreadablePages(docEntry, fileEntry.RelativePathAndFile()));
                                BaseDocObject.JustPassedTreshold = false;
                                //return new DocumentsTiffUnreadablePages(new DocIndexEntry(), "testing");
                                break;
                            }
                        }
                        log.Error($"File '{bufferedProgressStream.file.FullName}' caused an error! ", e);
                        Report(new AdaAvInternalError(e, GetType()));
                    }
                }
            }

            ReportAny(documentsTiffPrivateTagsSummary.Report());
        }

        protected override void OnRun(FullDocumentRepoHacker documentIndexRepo)
        {
            BufferedProgressStream.BufferingEnabled = true;
            BufferedProgressStream.MaximumFileSizeBuffered = 50 * 1024 * 1024;
            BufferedProgressStream.MaximumFilesBuffered = 10;
            BufferedProgressStream.MaximumTotalBufferSize = BufferedProgressStream.MaximumFileSizeBuffered
                                                            * BufferedProgressStream.MaximumFilesBuffered / 2;

            var totalDoc = documentIndexRepo.DocFilesCount();
            var docI = 0L;

            void Reporter()
            {
                ++docI;
                ProgressCallback?.Invoke($"{docI} ud af {totalDoc}");
            }

            IngestBasic(
                documentIndexRepo,
                FileTypeEnum.Tif,
                (stream, info, callBack) =>
                {
                    using (new TiffObject(stream, info, callBack, _checks, _highPageCount))
                    {
                    }
                }, Reporter
            );

            IngestBasic(
                documentIndexRepo,
                FileTypeEnum.Jp2,
                (stream, info, callBack) =>
                {
                    using (new Jp2Object(stream, info, callBack))
                    {
                    }
                }, Reporter);
            IngestBasic(
                documentIndexRepo,
                FileTypeEnum.Mpg,
                (stream, info, callBack) =>
                {
                    using (new MpgObject(stream, info, callBack, _checks))
                    {
                    }
                }, Reporter);
            IngestBasic(
                documentIndexRepo,
                FileTypeEnum.Mp3,
                (stream, info, callBack) =>
                {
                    using (new Mp3Object(stream, info, callBack, _checks))
                    {
                    }
                }, Reporter);
            IngestBasic(
                documentIndexRepo,
                FileTypeEnum.Wav,
                (stream, info, callBack) =>
                {
                    using (new WaveObject(stream, info, callBack, _checks))
                    {
                    }
                }, Reporter);
        }

        #endregion
    }
}