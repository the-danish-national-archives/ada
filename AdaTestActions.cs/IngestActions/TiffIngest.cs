using Ra.EntityExtensions.DocumentExtensions;

namespace Ada.Common.IngestActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using global::Ada.ADA.Common;
    using global::Ada.Core;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using log4net;

    using Ra.DomainEntities;
    using Ra.DomainEntities.Documents;
    using Ra.Tiffery;

    public class TiffIngest : AdaActionBase<DocumentIndexRepo>
    {   
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IEnumerable<DocIndexEntry> docIndexEntries;

        //private readonly ITiffLeadToolsProcessorWithStreams tiffery;

        public TiffIngest(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {

        }

        protected override void OnRun(DocumentIndexRepo documentIndexRepo)
        {
            
            // TODO filter out files not existing
            var files = documentIndexRepo.EnumerateDocs(FileTypeEnum.Tif);

            foreach (var entry in files)
            {
                var pathToFile = Path.Combine(
                    this.Mapping.GetMediaPath(Int32.Parse(entry.MediaId)),
                    entry.RelativePathAndFile());
                if (!(new FileInfo(pathToFile)).Exists)
                {
                    this.log.Debug($"File \'{pathToFile}\' not found!");
                    continue; // caught in structure test
                }

                var bufferedProgressStream = entry.GetAsStream(this.Mapping);

                if (TiffLeadToolsProcessorWithStreams.VerifyTiffByMagicNumber(bufferedProgressStream))
                    //TODO Skal flyttes højere op til kald af DocumentIngestAction
                {
                    try
                    {
                        var di = new DocInfo
                                     {
                                         DocumentId = entry.DocumentId,
                                         DocumentFolder = entry.DocumentFolder,
                                         MediaId = entry.MediaId
                                     };

                        var tiffObject = new TiffObject(
                            bufferedProgressStream,
                            di,
                            tiffLogEntry =>
                                {
                                    var logEntry = new LogEntry()
                                                       {
                                                           EntryTypeId = tiffLogEntry.EventTypeId,
                                                           EntryId = tiffLogEntry.EventId,
                                                       };

                                    logEntry.EntryTags =
                                        tiffLogEntry.EntryTags.Select(
                                            t =>
                                            new LogEntryTag()
                                                {
                                                    TagId = t.TagId,
                                                    ParentEntry = logEntry,
                                                    TagText = t.TagText,
                                                    TagType = t.TagType
                                                }).ToList();
                                    this.ReportLogEntry(logEntry);
                                });
                    }
                    catch (Exception e)
                    {
                        this.log.Error($"File '{bufferedProgressStream.file.FullName}' caused an error! ", e);
                        throw;
                    }
                }
            }
        }
    }
}