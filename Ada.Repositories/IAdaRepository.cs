namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion

    public interface IAdaRepository
    {
    }

    public interface IAdaEntityRepository<out TEntity> : IAdaRepository
    {
        #region

        TEntity LoadEntity();

        #endregion
    }

    public interface IAdaEnumerableEntityRepository<out TEntity> : IAdaRepository
    {
        #region

        IEnumerable<TEntity> EnumerateEntities();

        #endregion
    }

    public interface IAdaIngestRepository<in TEntity> : IAdaRepository
    {
        #region

        void Clear();
        void SaveEntity(TEntity entity);

        #endregion
    }

    public interface IAdaRepoType<TType> : IAdaRepository
        where TType : IRepoType
    {
    }

    public interface IRepoType
    {
    }

    public interface IRepoTypeTest : IRepoType
    {
    }

    public interface IRepoTypeAv : IRepoType
    {
    }

    public interface IAdaSqlRepo : IAdaRepository
    {
        #region

        long AsCountExecuteScalar(string query);

        long ExecuteScalar(string query);

        T QueryEnumerator<T>(string query, Func<IEnumerable<IDataReader>, T> func);

        #endregion
    }
}