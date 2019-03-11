// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="">
//   
// </copyright>
// <summary>
//   The UnitOfWork interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository
{
    #region Namespace Using

    using System;
    using global::NHibernate;

    #endregion

    /// <summary>
    ///     The UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region

        /// <summary>
        ///     The begin transaction.
        /// </summary>
        /// <returns>
        ///     The <see cref="ITransaction" />.
        /// </returns>
//        ITransaction 
        void
            BeginTransaction();

        /// <summary>
        ///     The commit.
        /// </summary>
        void Commit();

        /// <summary>
        ///     The get repository.
        /// </summary>
        /// <typeparam name="TEntity">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IReadWriteRepository" />.
        /// </returns>
        IReadWriteRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        ///     The rollback.
        /// </summary>
        void Rollback();

        #endregion
    }
}