namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using Ra.Common.Repository.NHibernate;

    #endregion

    public
//        abstract 
        class RepoBase : IDisposable, IAdaRepository
    {
        #region  Fields

        protected readonly IAdaUowFactory adaUowFactory;

        private UnitOfWork _uow;

        private readonly int CommitInterval;


        private bool disposed;

//        private IDbConnection _connection = null;


//        private IDbTransaction Transaction;


        private int OperationsSinceLastCommit;

        #endregion

        #region  Constructors

//        private ITransaction _transaction;

//        public RepoBase() { }

        public RepoBase(IAdaUowFactory adaUowFactory, int commitInterval)
        {
//            AViD aviD, string dbCreationFolder,
            this.adaUowFactory = adaUowFactory;
            CommitInterval = commitInterval;
            OperationsSinceLastCommit = 0;
        }

        #endregion

        #region IDisposable Members

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region

        public virtual void Clear<TEntity>()
        {
            adaUowFactory.CleanUpTable<TEntity>();
        }

        public void Commit(bool keepAlive = false)
        {
            if (_uow == null)
                return;
            try
            {
                _uow.Commit();
                _uow.BeginTransaction();
                OperationsSinceLastCommit = 0;
            }
            finally
            {
                if (!keepAlive) StopConnection();
            }
        }

        protected void CommitPerhaps()
        {
            OperationsSinceLastCommit++;

            if (CommitInterval == 0) return;
            if (OperationsSinceLastCommit > CommitInterval) Commit(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (CommitInterval != 0) Commit();

                    if (_uow != null)
                        throw new InvalidOperationException("Did you forget to commit?");
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.


                disposed = true;
            }
        }

        public IEnumerable<IDataReader> EnumerateQuery(string query)
        {
//            IDbConnection connection;
            var started = StartConnection();

            try
            {
                using (var cmd = _uow.Session.Connection.CreateCommand())
                {
                    cmd.CommandText = query;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) yield return reader;
                    }
                }
            }
            finally
            {
                if (started && OperationsSinceLastCommit == 0)
                    StopConnection();
            }
        }


        protected void ExecuteNonQuery(string commandText)
        {
            StartConnection();

            using (var cmd = _uow.Session.Connection.CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.ExecuteNonQuery();
            }

            CommitPerhaps();
        }

        protected object ExecuteScalarQuery(string commandText, bool withCommitPerhaps = false)
        {
            return WithConnection(
                c =>
                {
                    using (var cmd = c.CreateCommand())
                    {
                        cmd.CommandText = commandText;
                        return cmd.ExecuteScalar();
                    }
                }, withCommitPerhaps);
        }

        // Use C# destructor syntax for finalization code.
        ~RepoBase()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        //public abstract IEnumerable<T> EnumerateFiles();

        //public abstract void AddFile(T file);


        public T QueryEnumerator<T>(string query, Func<IEnumerable<IDataReader>, T> func)
        {
            var started = StartConnection();

            try
            {
                using (var cmd = _uow.Session.Connection.CreateCommand())
                {
                    cmd.CommandText = query;

                    using (var reader = cmd.ExecuteReader())
                    {
                        return func(QueryEnumeratorHelper(reader));
                    }
                }
            }
            finally
            {
                if (started && OperationsSinceLastCommit == 0)
                    StopConnection();
            }
        }

        private IEnumerable<IDataReader> QueryEnumeratorHelper(IDataReader reader)
        {
            while (reader.Read()) yield return reader;
        }

        private bool StartConnection()
        {
            if (_uow != null)
                return false;
            _uow = (UnitOfWork) adaUowFactory.GetUnitOfWork();
//            _connection = _uow.Session.Connection;

            _uow.BeginTransaction();
            return true;
        }

        private void StopConnection()
        {
            _uow.Dispose();
            _uow = null;
//            this._connection = null;
        }

        protected T WithConnection<T>(Func<IDbConnection, T> func, bool withCommitPerhaps = false)
        {
            var started = StartConnection();

            try
            {
                return func(_uow.Session.Connection);
            }
            finally
            {
                if (withCommitPerhaps)
                {
                    CommitPerhaps();
                }
                else
                {
                    if (started && OperationsSinceLastCommit == 0)
                        StopConnection();
                }
            }
        }

        #endregion
    }
}