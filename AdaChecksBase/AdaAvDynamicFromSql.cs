namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ra.Common.Repository.NHibernate;
    using Repositories;

    #endregion

    public abstract class AdaAvDynamicFromSql : AdaAvViolation // AdaAvCheckNotification
    {
        #region Static

        private static readonly Type _myType = typeof(AdaAvDynamicFromSql);

        #endregion

        #region  Fields

        private IDictionary<string, object> tags;

        #endregion

        #region  Constructors

        protected AdaAvDynamicFromSql(string tagType)
            : base(tagType)
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvDynamicFromSql> CheckInstance(Type type, IAdaUowFactory testFactory)
        {
            if (!type.IsSubclassOf(_myType))
                throw new ArgumentOutOfRangeException(
                    nameof(type),
                    $"Must be a sub class of {_myType.AssemblyQualifiedName}");

            var checkInstance = (AdaAvDynamicFromSql) Activator.CreateInstance(type, true);

            var res = new List<AdaAvDynamicFromSql>();

            using (var uow = (UnitOfWork) testFactory.GetUnitOfWork())
            {
//                uow.BeginTransaction();


                var query = checkInstance.GetTestQuery(type);


                var results = uow.Session.CreateSQLQuery(query).DynamicList();

                foreach (var result in results)
                {
                    var resInstance = (AdaAvDynamicFromSql) checkInstance.MemberwiseClone();
                    resInstance.tags = result;

                    res.Add(resInstance);
                }

//                uow.
//                uow.Commit();
            }

            return res;
        }

        protected override IEnumerable<Tuple<string, string>> GetEntryTags()
        {
            return tags.Select(p => new Tuple<string, string>(p.Key, p.Value.ToString()));
        }

        protected abstract string GetTestQuery(Type type);

        #endregion

//        protected abstract IEnumerable<dynamic> GetQuery();
    }

    //    public class AdaAvDynamicFromSqlQueryAttribute : Attribute
    //    {
    //        public string Query { get; set; }
    //
    //        public AdaAvDynamicFromSqlQueryAttribute(string query)
    //        {
    //            Query = query;
    //        }
    //    }
//    public static class IAdaUowFactoryExtension
//    {
//
//        public static IEnumerable<AdaAvDynamicFromSql> CheckInstance(this IAdaUowFactory testFactory, Type type)
//        {
//            return AdaAvDynamicFromSql.CheckInstance(type, testFactory);
//        }
//    }
}