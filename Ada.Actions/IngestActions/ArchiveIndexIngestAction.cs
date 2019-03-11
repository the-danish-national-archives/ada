namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using ChecksBase;
    using Common;
    using EntityLoaders;
    using Log;
    using Ra.Common.Xml;
    using Ra.DomainEntities.ArchiveIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("files")]
    [ReportsChecks(typeof(ArchiveIndexNotWellFormed),
        typeof(ArchiveIndexInvalid),
        typeof(AdaAvXmlIndexIllegalEncoding),
        typeof(AdaAvXmlIndexMissingProlog),
        typeof(ArchiveVersionViolation),
        typeof(ArchiveIndexArchivePeriodViolation),
        typeof(ArchiveIndexCreationPeriodViolation),
        typeof(ArchiveIndexRelatedRecordsNameInvalid),
        typeof(ArchiveIndexRelatedRecordsNamesMissing),
        typeof(ArchiveIndexRelatedRecordsNamesMisMatch)
    )]
    public class ArchiveIndexIngestAction : AdaXmlIngestAction<ArchiveIndexRepo, ArchiveIndex>
    {
        #region  Constructors

        public ArchiveIndexIngestAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            ArchiveIndexRepo targetRepository)
            : base(processLog, testLog, GetLogger(mapping), mapping, targetRepository)
        {
        }

        #endregion

        #region

        private static IXmlEventLogger GetLogger(AVMapping localMapping)
        {
            var errorMap = new Dictionary<XmlEventType, Type>
            {
                {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                {XmlEventType.XmlWellFormednessError, typeof(ArchiveIndexNotWellFormed)},
                {XmlEventType.XmlValidationError, typeof(ArchiveIndexInvalid)},
                {XmlEventType.XmlValidationWarning, typeof(ArchiveIndexInvalid)},
                {XmlEventType.XmlDeclaredEncodingIllegal, typeof(AdaAvXmlIndexIllegalEncoding)},
                {XmlEventType.XmlMissingProlog, typeof(AdaAvXmlIndexMissingProlog)}
            };

            IXmlEventLogger logger = new XmlEventLogger(
                errorMap,
                localMapping);

            return logger;
        }

        protected override void OnRun(XmlCouplet targetCouplet)
        {
            Logger.CallBack = Report;
            var index = ArchiveIndexLoader.Load(ArchivalXmlReader, targetCouplet.XmlStream, targetCouplet.SchemaStream);

            TargetRepository.SaveEntity(index);

            ReportAny(ArchiveVersionViolation.Check(index, Mapping.AVID));
            ReportAny(ArchiveIndexArchivePeriodViolation.Check(index));
            ReportAny(ArchiveIndexCreationPeriodViolation.Check(index));
            ReportAny(ArchiveIndexRelatedRecordsNamesMissing.Check(index));
            ReportAny(ArchiveIndexRelatedRecordsNameInvalid.Check(index));
            ReportAny(ArchiveIndexRelatedRecordsNamesMisMatch.Check(index));
        }

        #endregion
    }
}