// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="">
// </copyright>
// <summary>
//   The unit of work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository.NHibernate
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using global::NHibernate;

    #endregion

    /// <summary>
    ///     The unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Static

        private static readonly List<WeakReference> _allActiveUnitOfWorks =
            new List<WeakReference>();

        #endregion

        #region  Fields

        /// <summary>
        ///     The _repositories.
        /// </summary>
        private readonly Dictionary<Type, object> _repositories;

        /// <summary>
        ///     The _session factory.
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        ///     The _transaction.
        /// </summary>
        private ITransaction _transaction;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="sessionFactory">
        ///     The session factory.
        /// </param>
        private UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            Session = _sessionFactory.OpenSession();
            _repositories = new Dictionary<Type, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the session.
        /// </summary>
        public ISession Session { get; }

        #endregion

        #region IUnitOfWork Members

        /// <summary>
        ///     The begin transaction.
        /// </summary>
        /// <returns>
        ///     The <see cref="ITransaction" />.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public void BeginTransaction()
        {
            if (_transaction != null) throw new InvalidOperationException("Cannot have more than one transaction per session.");

            _transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        ///     The commit.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction opened.");

            if (!_transaction.IsActive)
                throw new InvalidOperationException("Cannot commit to inactive transaction.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            Session.Dispose();

            _transaction?.Dispose();
        }

        /// <summary>
        ///     The get repository.
        /// </summary>
        /// <typeparam name="TEntity">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IReadWriteRepository{TEntity}" />.
        /// </returns>
        public IReadWriteRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            foreach (var key in _repositories.Keys)
                if (key == typeof(TEntity))
                    return _repositories[typeof(TEntity)] as IReadWriteRepository<TEntity>;

            var repository = new Repository<TEntity>(Session);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        /// <summary>
        ///     The rollback.
        /// </summary>
        public void Rollback()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction open.");

            if (_transaction.IsActive) _transaction.Rollback();
        }

        #endregion

        #region

        private static UnitOfWork AddUnitOfWork(UnitOfWork unitOfWork)
        {
            lock (_allActiveUnitOfWorks)
            {
                var capacity = _allActiveUnitOfWorks.Capacity;
                if (_allActiveUnitOfWorks.Count == _allActiveUnitOfWorks.Capacity) ClearOutAllActiveUnitOfWorks();

                _allActiveUnitOfWorks.Add(new WeakReference(unitOfWork));
            }

            return unitOfWork;
        }

        private static void ClearOutAllActiveUnitOfWorks()
        {
            _allActiveUnitOfWorks.RemoveAll(r => !r.IsAlive);
        }

        public static UnitOfWork CreateInstance(ISessionFactory sessionFactory)
        {
            return AddUnitOfWork(new UnitOfWork(sessionFactory));
        }

        public static void DisposeAll()
        {
            lock (_allActiveUnitOfWorks)
            {
                foreach (var r in _allActiveUnitOfWorks)
                {
                    var unitOfWork = r.Target as UnitOfWork;
                    unitOfWork?.Dispose();
                }
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        #endregion
    }
}