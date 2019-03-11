//using System.Collections.Generic;

//namespace Ada.ADA.Common.TestActions
//{
//    using System.Linq;

//    using global::Ada.Common;
//    using global::Ada.Log;
//    using global::Ada.Log.Entities;
//    using global::Ada.Repositories;

//    using Ra.Common.Repository.NHibernate;
//    using Ra.DomainEntities.TableIndex;

//    public class DocumentSystemFileRelationTest : AdaActionBase
//    {
//        private readonly IAdaUowFactory testFactory;

//        public DocumentSystemFileRelationTest(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory)
//            : base(processLog, testLog)
//        {
//            this.testFactory = testFactory;
//        }

//        protected void OnRun(Job job)
//        {
//            using (var testuow = (UnitOfWork)this.testFactory.GetUnitOfWork())
//            {
//                var tables = testuow.GetRepository<Table>().All().ToList();
//            }
//        }
//    }
//}
