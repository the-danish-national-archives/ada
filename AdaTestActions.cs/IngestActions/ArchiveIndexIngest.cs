namespace Ada.Common.IngestActions
{
    using System;

    using global::Ada.ADA.EntityLoaders;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.Xml;

    using Ra.Common;
    using Ra.DomainEntities.ArchiveIndex;

    public class ArchiveIndexIngest : AdaXmlIngest
    {
        private readonly IAdaUowFactory testFactory;

        public ArchiveIndexIngest(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, IArchivalXmlReader reader, IXmlEventLogger logger, AVMapping mapping)
            : base(processLog, testLog, reader, logger, mapping)
        {
            this.testFactory = testFactory;
        }

        protected override void OnRun(XmlCouplet targetCouplet)
        {
            var index = ArchiveIndexLoader.Load(this.ArchivalXmlReader, targetCouplet.XmlStream, targetCouplet.SchemaStream);

            using (var uow = testFactory.GetUnitOfWork())
            {
                uow.BeginTransaction();

                var repo = uow.GetRepository<ArchiveIndex>(); 
                repo.Add(index);

                uow.Commit();

                if (!index.ArchiveInformationPackageId.Equals(this.Mapping.AVID.FullID, StringComparison.OrdinalIgnoreCase))
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.A_1" };
                    logEntry.AddTag("ActualId", this.Mapping.AVID.FullID);
                    logEntry.AddTag("AnnotatedId", index.ArchiveInformationPackageId);
                    this.ReportLogEntry(logEntry);
                }
            }
        }
    }
}