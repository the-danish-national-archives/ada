namespace Ada.Repositories
{
    #region Namespace Using

    using Ra.Common.Repository;

    #endregion

    public interface IAdaUowFactory
    {
        #region

        void CleanUpTable<T>();
        void CreateDataBase();
        bool DataBaseExists();
        bool DeleteDataBase();
        IUnitOfWork GetUnitOfWork();

        #endregion
    }
}