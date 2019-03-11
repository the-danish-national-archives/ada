// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextDocumentationIngest.cs" company="">
//   
// </copyright>
// <summary>
//   The Context documentation ingest action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ada.Common.IngestActions
{
    using global::Ada.ADA.EntityLoaders;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common;
    using Ra.Common.Xml;
    using Ra.DomainEntities.ContextDocumentationIndex;
    using Ra.EntityExtensions.ContextDocumentationIndex;

    /// <summary>
    /// The Context documentation ingest action.
    /// </summary>
    public class ContextDocumentationIngest : AdaXmlIngest
    {
        private readonly IAdaUowFactory testFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextDocumentationIngest"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ContextDocumentationIngest(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, IArchivalXmlReader reader, IXmlEventLogger logger, AVMapping mapping)
            : base(processLog, testLog, reader, logger, mapping)
        {
            this.testFactory = testFactory;
        }

        /// <summary>
        /// The run.
        /// </summary> 
        /// <param name="job">
        /// The job.
        /// </param>
        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            var index = ContextDocumentationIndexLoader.Load(this.ArchivalXmlReader, targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream); 


            using (var uow = this.testFactory.GetUnitOfWork())
            {
                uow.BeginTransaction();

                var repo = uow.GetRepository<ContextDocumentationDocument>();

                repo.Add(index.Documents);

                uow.Commit();

                foreach(var doc in index.Documents)
                {
                    if (!doc.DocumentCategory.IsAnnotated())
                    {
                        var logEntry = new LogEntry { EntryTypeId = "4.C.4_1" };
                        logEntry.AddTag("DocId", doc.DocumentId);
                        logEntry.AddTag("Title", doc.DocumentTitle);
                        this.ReportLogEntry(logEntry);
                    }

                }
            }
        }
    }
}