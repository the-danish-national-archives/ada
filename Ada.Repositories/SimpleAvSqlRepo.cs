#region Header

// Author 
// Created 19

#endregion

namespace Ada.Repositories
{
    public class SimpleAvSqlRepo : RepoBase, IAdaSqlRepo, IAdaRepoType<IRepoTypeAv>
    {
        #region  Constructors

        public SimpleAvSqlRepo(IAdaUowFactory adaUowFactory)
            :
            base(adaUowFactory, 0)
        {
        }

        #endregion

        #region IAdaSqlRepo Members

        public long AsCountExecuteScalar(string query)
        {
            return (long) ExecuteScalarQuery($"SELECT COUNT(1) FROM ({query})");
        }

        public long ExecuteScalar(string query)
        {
            return (long) ExecuteScalarQuery($"{query}");
        }

        #endregion
    }
}