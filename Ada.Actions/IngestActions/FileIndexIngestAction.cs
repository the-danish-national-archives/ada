namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using ActionBase;
    using Checks;
    using Checks.FileIndex;
    using ChecksBase;
    using Common;
    using Log;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Xml;
    using Ra.DomainEntities;
    using Ra.DomainEntities.FileIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("files")]
    [ReportsChecks(
        typeof(FileIndexNotWellFormed),
        typeof(FileIndexInvalid),
        typeof(AdaAvXmlIndexIllegalEncoding),
        typeof(AdaAvXmlIndexMissingProlog),
        typeof(FileIndexAvid)
    )]
    public class FileIndexIngestAction : AdaXmlIngestAction<AdaFileIndexRepo, FileIndexEntry>
    {
        #region  Constructors

        public FileIndexIngestAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            AdaFileIndexRepo targetRepository)
            : base(processLog, testLog, GetLogger(mapping), mapping, targetRepository)
        {
        }

        #endregion

        #region

        public IEnumerable<FileIndexEntry> EnumerateFileIndexEntries(XmlCouplet targetXmlCouplet)
        {
            ArchivalXmlReader.Open(targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);
            foreach (var element in (ArchivalXmlReader as ArchivalXmlReader).ElementStream("f"))
            {
                XNamespace ns = "http://www.sa.dk/xmlns/diark/1.0";

                var folderName = element.Element(ns + "foN")?.Value;
                var root = folderName.GetRootFolder();

                var fileName = element.Element(ns + "fiN")?.Value;


                ReportAny(FileIndexAvid.Check(Mapping, root, folderName + "\\" + fileName));


                var file = new FileIndexEntry
                {
                    MediaNumber = AViD.ExtractMediaNumber(root),
                    RelativePath = folderName?.Replace(root, string.Empty),
                    foN = folderName,
                    FileName = Path.GetFileNameWithoutExtension(fileName),
                    Extension = Path.GetExtension(fileName),
                    Md5 = element.Element(ns + "md5")?.Value
                };
                yield return file;
            }

            ArchivalXmlReader.Close();
        }

//        
        private static IXmlEventLogger GetLogger(AVMapping localMapping)
        {
            var errorMap = new Dictionary<XmlEventType, Type>
            {
                {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                {XmlEventType.XmlWellFormednessError, typeof(FileIndexNotWellFormed)},
                {XmlEventType.XmlValidationError, typeof(FileIndexInvalid)},
                {XmlEventType.XmlValidationWarning, typeof(FileIndexInvalid)},
                {XmlEventType.XmlDeclaredEncodingIllegal, typeof(AdaAvXmlIndexIllegalEncoding)},
                {XmlEventType.XmlMissingProlog, typeof(AdaAvXmlIndexMissingProlog)}
            };

            IXmlEventLogger logger = new XmlEventLogger(
                errorMap,
                localMapping);

            return logger;
        }

        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            Logger.CallBack = Report;
            foreach (var file in EnumerateFileIndexEntries(targetXmlCouplet)) TargetRepository.SaveEntity(file);
            TargetRepository.Commit();
        }

        #endregion
    }
}