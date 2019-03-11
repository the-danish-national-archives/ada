using System;
using System.Collections.Generic;

namespace Ada.Common.IngestActions
{
    using System.Linq;

    using global::Ada.ActionBase;
    using global::Ada.Log;

    public class AdaSingleQuery : AdaActionBase<string>
    {
        private readonly IAdaUowFactory testFactory;

        public AdaSingleQuery(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping) : base(processLog, testLog,mapping)
        {
            this.testFactory = testFactory;
        }

        protected override void OnRun(string testSubject)
        {
            using (UnitOfWork uow = (UnitOfWork)testFactory.GetUnitOfWork())
            {
                uow.BeginTransaction();
                var repo = uow.GetRepository<TestQuery>();
                var query = repo.FindBy(x => x.TestId == testSubject);

                var results = uow.Session.CreateSQLQuery(query.Query).DynamicList();

                foreach (var result in results)
                {
                    var logEntry = new LogEntry();
                    logEntry.EntryTypeId = testSubject;

                    foreach (var property in (IDictionary<String, object>)result)
                    {
                        logEntry.AddTag(property.Key, property.Value.ToString());
                    }
                    this.ReportLogEntry(logEntry);
                }
                uow.Commit();
            }
        }
    }
} 