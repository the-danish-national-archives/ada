namespace Ada.Repositories
{
    #region Namespace Using

    using System.IO;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Log.EntityMappings;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;
    using Ra.DomainEntities;

    #endregion

    public class AdaLogUowFactory : AdaBaseUowFactory
    {
        #region  Fields

        private Configuration savedSchema;

        #endregion

        #region  Constructors

        public AdaLogUowFactory(AViD id, string extension, DirectoryInfo path)
            : base(id, extension, path)
        {
        }

        #endregion

        #region Properties

        private ISession Session { get; set; }

        #endregion

        #region

        private void buildSchema(ISession session)
        {
//            new SchemaUpdate(savedSchema).Execute(false, true);
            new SchemaExport(savedSchema).Execute(false, true, false, session.Connection, null);
        }


        protected override ISessionFactory MakeSessionFactory()
        {
            var sessionFactory =
                Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionStringBuilder.ToString()))
                    .Mappings(
                        m =>
                            m.FluentMappings.AddFromAssemblyOf<LogEntryMap>()).ExposeConfiguration(
                        cfg =>
                        {
                            if (inMemoryDatabase)
                                savedSchema = cfg;
                            else
                                new SchemaUpdate(cfg).Execute(false, true);
                        })
                    .BuildSessionFactory();

            if (inMemoryDatabase)
            {
                Session = sessionFactory.OpenSession();
                buildSchema(Session);
            }

            return sessionFactory;
        }

        #endregion
    }
}