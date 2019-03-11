namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using ActionBase;
    using Checks;
    using Checks.DocIndex;
    using ChecksBase;
    using Common;
    using Log;
    using Ra.Common.Xml;
    using Ra.DomainEntities.Documents;
    using Repositories;

    #endregion

    [AdaActionPrecondition("files")]
    [ReportsChecks(typeof(AdaAvXmlViolation),
        typeof(DocIndexNotWellFormed),
        typeof(DocIndexInvalid),
        typeof(AdaAvInternalError),
        typeof(AdaAvXmlIndexIllegalEncoding),
        typeof(AdaAvXmlIndexMissingProlog))]
    public class DocumentIndexIngestAction : AdaXmlIngestAction<DocumentIndexRepo, DocIndexEntry>
    {
        #region  Constructors

        public DocumentIndexIngestAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            DocumentIndexRepo targetRepository)
            : base(processLog, testLog, GetLogger(mapping), mapping, targetRepository)
        {
        }

        #endregion

        #region

        public IEnumerable<DocIndexEntry> EnumerateDocIndexEntries(XmlCouplet targetXmlCouplet)
        {
            ArchivalXmlReader.Open(targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);
            foreach (var element in (ArchivalXmlReader as ArchivalXmlReader).ElementStream("doc"))
            {
                XNamespace ns = "http://www.sa.dk/xmlns/diark/1.0";
                var doc = new DocIndexEntry
                {
                    DocumentId = element.Element(ns + "dID")?.Value,
                    ParentId =
                        element.Element(ns + "pID")?
                            .Attribute("{http://www.w3.org/2001/XMLSchema-instance}nil") == null
                            ? element.Element(ns + "pID")?.Value
                            : null,
                    MediaId = element.Element(ns + "mID")?.Value,
                    DocumentFolder = element.Element(ns + "dCf")?.Value,
                    OriginalFileName = element.Element(ns + "oFn")?.Value.Replace("\'", "''"),
                    SubmissionFileType = element.Element(ns + "aFt")?.Value,
                    GmlXsd = element.Element(ns + "gmlXsd")?.Value
                };

                doc.Extension = getFileExtension(doc.OriginalFileName);

                yield return doc;
            }

            ArchivalXmlReader.Close();
        }

        private string getFileExtension(string fileName)
        {
            var pos = fileName.LastIndexOf(".");
            if (pos >= 0) return fileName.Substring(pos);
            return string.Empty;
        }

        private static IXmlEventLogger GetLogger(AVMapping localMapping)
        {
            var errorMap = new Dictionary<XmlEventType, Type>
            {
                {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                {XmlEventType.XmlWellFormednessError, typeof(DocIndexNotWellFormed)},
                {XmlEventType.XmlValidationError, typeof(DocIndexInvalid)},
                {XmlEventType.XmlValidationWarning, typeof(DocIndexInvalid)},
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
            foreach (var doc in EnumerateDocIndexEntries(targetXmlCouplet)) TargetRepository.SaveEntity(doc);
            TargetRepository.Commit();
        }

        #endregion
    }
}