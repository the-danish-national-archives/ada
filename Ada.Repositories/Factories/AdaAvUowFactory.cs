namespace Ada.Repositories
{
    #region Namespace Using

    using System.IO;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using Ra.DomainEntities;

    #endregion

    public class AdaAvUowFactory : AdaBaseUowFactory
    {
        #region  Constructors

        public AdaAvUowFactory(AViD id, string extension, DirectoryInfo path)
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
                    //.Mappings(
                    //    m =>
                    //    m.FluentMappings.AddFromAssemblyOf<Ada.Log.EntityMappings.LogEntryMap>())
                    //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory();

            return sessionFactory;
        }

        #endregion
    }
}