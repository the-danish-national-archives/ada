namespace Ada.Repositories
{
    #region Namespace Using

    using System.IO;
    using EntityMaps;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Tool.hbm2ddl;
    using Ra.DomainEntities;

    #endregion

    public class AdaTestUowFactory : AdaBaseUowFactory
    {
        #region  Constructors

        public AdaTestUowFactory(AViD id, string extension, DirectoryInfo path)
            : base(id, extension, path)
        {
        }

        #endregion

        #region

        protected override ISessionFactory MakeSessionFactory()
        {
            var sessionFactory =
                Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionStringBuilder.ToString()))
                    .Mappings(
                        m =>
                            m.FluentMappings.AddFromAssemblyOf<TestQueryMap>())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory();

            return sessionFactory;
        }

        #endregion
    }
}