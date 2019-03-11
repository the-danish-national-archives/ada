namespace Ada.Actions
{
    #region Namespace Using

    using System;
    using ActionBase;
    using ChecksBase;
    using Common;
    using Log;
    using Repositories;

    #endregion

    public class AdaSingleQueryAction : AdaActionBase<Type>
    {
        #region  Fields

        private readonly IAdaUowFactory testFactory;

        #endregion

        #region  Constructors

        public AdaSingleQueryAction(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping) : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
        }

        #endregion

        #region

//        public Type Type { get; set; }
//
//        public AdaSingleQueryAction(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, Type type) : base(processLog, testLog)
//        {
//            this.testFactory = testFactory;
//            var required = typeof(AdaAvDynamicFromSql);
//            if (!type.IsSubclassOf(required))
//                throw new ArgumentOutOfRangeException(nameof(type), $"Must be a sub class of {required.AssemblyQualifiedName}");
//            Type = type;
//        }


        protected override void OnRun(Type type)
        {
            var required = typeof(AdaAvDynamicFromSql);

            if (!type.IsSubclassOf(required))
                throw new ArgumentOutOfRangeException(nameof(type), $"Must be a sub class of {required.AssemblyQualifiedName}");

            foreach (var ret in AdaAvDynamicFromSql.CheckInstance(type, testFactory)) Report(ret);

            //            using (UnitOfWork uow = (UnitOfWork)this.testFactory.GetUnitOfWork())
            //            {
            //                uow.BeginTransaction();
            //                var repo = uow.GetRepository<TestQuery>();
            //                var query = repo.FindBy(x => x.TestId == testSubject);
            //
            //                var results = uow.Session.CreateSQLQuery(query.Query).DynamicList();
            //
            //                foreach (var result in results)
            //                {
            //                    var logEntry = new LogEntry();
            //                    logEntry.EntryTypeId = testSubject;
            //
            //                    foreach (var property in (IDictionary<String, object>)result)
            //                    {
            //                        logEntry.AddTag(property.Key, property.Value.ToString());
            //                    }
            //                    this.ReportLogEntry(logEntry);
            //                }
            //                uow.Commit();
            //            }
        }

        #endregion
    }
}