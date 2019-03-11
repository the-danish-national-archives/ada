// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The WriteRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The WriteRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IWriteRepository<in TEntity>
        where TEntity : class
    {
        #region

        /// <summary>
        ///     The add.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Add(TEntity entity);

        /// <summary>
        ///     The add.
        /// </summary>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Add(IEnumerable<TEntity> entities);

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Delete(TEntity entity);

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Delete(IEnumerable<TEntity> entities);

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Update(TEntity entity);

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Update(IEnumerable<TEntity> entities);

        #endregion
    }
}