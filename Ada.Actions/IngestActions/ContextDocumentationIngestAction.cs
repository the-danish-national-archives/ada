namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ActionBase;
    using Checks;
    using Checks.ContextDocIndex;
    using ChecksBase;
    using Common;
    using EntityLoaders;
    using Log;
    using Ra.Common.Xml;
    using Ra.DomainEntities.ContextDocumentationIndex;
    using Repositories;

    #endregion

    /// <summary>
    ///     The Context documentation ingest action.
    /// </summary>
    [AdaActionPrecondition("files")]
    [ReportsChecks(typeof(ContextDocumentationNotWellFormed),
        typeof(ContextDocumentationInvalid),
        typeof(AdaAvXmlIndexIllegalEncoding),
        typeof(AdaAvXmlIndexMissingProlog),
        typeof(ContextDocIndexDocMissingCategory))
    ]
    public class ContextDocumentationIngestAction :
        AdaXmlIngestAction<ContextDocumentationRepo, ContextDocumentationIndex>
    {
        #region  Constructors

        public ContextDocumentationIngestAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            ContextDocumentationRepo targetRepository)
            : base(processLog, testLog, GetLogger(mapping), mapping, targetRepository)
        {
        }

        #endregion

        #region

        private static IXmlEventLogger GetLogger(AVMapping localMapping)
        {
            var errorMap =
                new Dictionary<XmlEventType, Type>
                {
                    {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                    {XmlEventType.XmlWellFormednessError, typeof(ContextDocumentationNotWellFormed)},
                    {XmlEventType.XmlValidationError, typeof(ContextDocumentationInvalid)},
                    {XmlEventType.XmlValidationWarning, typeof(ContextDocumentationInvalid)},
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
            var index = ContextDocumentationIndexLoader.Load(
                ArchivalXmlReader,
                targetXmlCouplet.XmlStream,
                targetXmlCouplet.SchemaStream);

            TargetRepository.SaveEntity(index);

            foreach (var doc in index.Documents) ReportAny(ContextDocIndexDocMissingCategory.Check(doc));
        }

        #endregion
    }
}