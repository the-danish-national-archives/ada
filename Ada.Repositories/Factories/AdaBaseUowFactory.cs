namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using NHibernate;
    using NHibernate.Persister.Collection;
    using NHibernate.Persister.Entity;
    using Ra.Common.Repository;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;

    #endregion

    public abstract class AdaBaseUowFactory : IAdaUowFactory, IDisposable
    {
        #region  Fields

// private ISession memoryConnection;
        protected SQLiteConnectionStringBuilder connectionStringBuilder;
        protected bool inMemoryDatabase;

        private ISessionFactory sessionfactory;

        #endregion

        #region  Constructors

        protected AdaBaseUowFactory(AViD id, string extension, DirectoryInfo path)
        {
            var fileName = string.Concat(id.FullID, ".", extension);

            inMemoryDatabase = path == null;

            var fullPath = inMemoryDatabase ? ":memory:" : Path.Combine(path.FullName, fileName);

            connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                SyncMode = SynchronizationModes.Off,
                JournalMode = SQLiteJournalModeEnum.Off,
                BinaryGUID = false
            };

            if (inMemoryDatabase)
                connectionStringBuilder.FullUri = "file::memory:?cache=shared";
            else
                connectionStringBuilder.DataSource = fullPath;
        }

        #endregion

        #region IAdaUowFactory Members

        public IUnitOfWork GetUnitOfWork()
        {
            if (sessionfactory == null) sessionfactory = MakeSessionFactory();

            return UnitOfWork.CreateInstance(sessionfactory);
        }

        public void CreateDataBase()
        {
            if (inMemoryDatabase)
                return;

            if (!File.Exists(connectionStringBuilder.DataSource)) SQLiteConnection.CreateFile(connectionStringBuilder.DataSource);
        }

        public bool DataBaseExists()
        {
            if (inMemoryDatabase)
                return true;

            return File.Exists(connectionStringBuilder.DataSource);
        }

        public bool DeleteDataBase()
        {
            if (inMemoryDatabase)
                return true;

            try
            {
                File.Delete(connectionStringBuilder.DataSource);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CleanUpTable<T>()
        {
            var deleteAll = string.Empty;
            var tables = TablesForCleanup(typeof(T));

            foreach (var table in tables)
                using (var session = sessionfactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var deleteQuery = $"DELETE FROM \"{table}\";";
                        session.CreateSQLQuery(deleteQuery).ExecuteUpdate();
                        transaction.Commit();
                    }
                }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            sessionfactory?.Close();
            sessionfactory?.Dispose();
        }

        #endregion

        #region

        public static bool DataBaseExists(AViD id, string extension, DirectoryInfo path)
        {
            var inMemoryDatabase = path == null;
            if (inMemoryDatabase)
                return true;

            var fileName = string.Concat(id.FullID, ".", extension);

            var fullPath = Path.Combine(path.FullName, fileName);
            return File.Exists(fullPath);
        }

        protected abstract ISessionFactory MakeSessionFactory();

        protected Stack<string> TablesForCleanup(Type type, Stack<string> collectedTypes = null)
        {
            collectedTypes = collectedTypes ?? new Stack<string>();

            if (sessionfactory == null) sessionfactory = MakeSessionFactory();

            var persister = sessionfactory.GetClassMetadata(type) as AbstractEntityPersister;
            if (persister == null)
                return collectedTypes;
            if (!collectedTypes.Contains(persister.TableName))
                collectedTypes.Push(persister.TableName);

            foreach (var propertyName in persister.PropertyNames)
            {
                var index = persister.GetPropertyIndex(propertyName);
                var propertyType = persister.PropertyTypes[index];
                if (propertyType.IsEntityType)
                {
                    var subPersister =
                        sessionfactory.GetClassMetadata(propertyType.ReturnedClass) as AbstractEntityPersister;
                    if (subPersister != null && !collectedTypes.Contains(subPersister.TableName)) TablesForCleanup(propertyType.ReturnedClass, collectedTypes);
                }

                if (propertyType.IsCollectionType)
                {
                    var roleName = persister.Name + "." + propertyName;
                    var collectionPersister =
                        sessionfactory.GetCollectionMetadata(roleName) as AbstractCollectionPersister;
                    if (collectionPersister != null)
                        if (!collectedTypes.Contains(collectionPersister.TableName))
                        {
                            collectedTypes.Push(collectionPersister.TableName);
                            foreach (var subType in propertyType.ReturnedClass.GetGenericArguments().ToList()) TablesForCleanup(subType, collectedTypes);
                        }
                }
            }

            return collectedTypes;
        }

        #endregion
    }
}