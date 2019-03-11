#region Header

// Author 
// Created 19

#endregion

namespace Ada.ChecksBase
{
    #region Namespace Using

    using Repositories;

    #endregion

    public class AdaAvDynamicFromSqlRepo<TRepo> where TRepo : IAdaSqlRepo, IAdaRepoType<IRepoType>
    {
    }
}