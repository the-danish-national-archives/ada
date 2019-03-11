// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The ReadRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository
{
    #region Namespace Using

    using System;
    using System.Linq;
    using System.Linq.Expressions;

    #endregion

    /// <summary>
    ///     The ReadRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IReadRepository<TEntity>
        where TEntity : class
    {
        #region

        /// <summary>
        ///     The all.
        /// </summary>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        IQueryable<TEntity> All();

        /// <summary>
        ///     The filter by.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        ///     The find by.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        TEntity FindBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        ///     The find by.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        TEntity FindBy(object id);

        #endregion
    }
}