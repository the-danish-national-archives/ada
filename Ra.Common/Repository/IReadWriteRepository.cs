// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadWriteRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The ReadWriteRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository
{
    /// <summary>
    ///     The ReadWriteRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IReadWriteRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>
        where TEntity : class
    {
    }
}