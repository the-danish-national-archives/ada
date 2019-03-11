// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="">
//   
// </copyright>
// <summary>
//   The repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository.NHibernate
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using global::NHibernate;
    using global::NHibernate.Linq;

    #endregion

    /// <summary>
    ///     The repository.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class Repository<TEntity> : IReadWriteRepository<TEntity>
        where TEntity : class
    {
        #region  Fields

        /// <summary>
        ///     The _session.
        /// </summary>
        private readonly ISession _session;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository{TEntity}" /> class.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        public Repository(ISession session)
        {
            _session = session;
        }

        #endregion

        #region IReadWriteRepository<TEntity> Members

        /// <summary>
        ///     The add.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool Add(TEntity entity)
        {
            _session.Save(entity);
            return true;
        }

        /// <summary>
        ///     The add.
        /// </summary>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities) _session.Save(entity);

            return true;
        }

        /// <summary>
        ///     The all.
        /// </summary>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        public IQueryable<TEntity> All()
        {
            return _session.Query<TEntity>();
        }

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool Delete(TEntity entity)
        {
            _session.Delete(entity);
            return true;
        }

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities) _session.Delete(entity);

            return true;
        }

        /// <summary>
        ///     The filter by.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        public IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        /// <summary>
        ///     The find by.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        public TEntity FindBy(Expression<Func<TEntity, bool>> expression)
        {
            return FilterBy(expression).SingleOrDefault();
        }

        /// <summary>
        ///     The find by.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        public TEntity FindBy(object id)
        {
            return _session.Get<TEntity>(id);
        }

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool Update(TEntity entity)
        {
            _session.Update(entity);
            return true;
        }

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities) _session.Update(entity);

            return true;
        }

        #endregion
    }
}