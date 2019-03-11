namespace Ada.Repositories
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class FullDocumentRepo : IDisposable // : BaseRepo
    {
        #region  Fields

        private readonly UnitOfWork _uow;
        private readonly string docIdsQueryText;

        #endregion

        #region  Constructors

        public FullDocumentRepo(IAdaUowFactory testFactory, AViD aviD, string dbCreationFolder)
        {
            _uow = (UnitOfWork) testFactory.GetUnitOfWork();
            var columns =
                _uow.GetRepository<Column>()
                    .All()
                    .Select(x => x)
                    .Where(x => x.FunctionalDescriptions.Contains(FunctionalDescription.Dokumentidentifikation))
                    .ToList();


            docIdsQueryText =
                string.Join(" UNION ",
                    columns.Select(x => $"SELECT {x.Name} AS A FROM testDB.{x.ParentTable.Name} WHERE {x.Name} IS NOT NULL"));

            Connection = _uow.Session.Connection;
            var cmd = Connection.CreateCommand();
            var path = Path.Combine(dbCreationFolder, aviD.FullID + ".av");

            cmd.CommandText = "ATTACH '" + path + "' AS testDB";
            cmd.ExecuteNonQuery();

            _uow.BeginTransaction();
        }

        #endregion

        #region Properties

        protected IDbConnection Connection { get; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //            this.Transaction.Dispose();
            //            this.Connection.Dispose();


            // disposes the others
            _uow.Dispose();
        }

        #endregion

        #region

        public long DocIdCountFromDocIndex()
        {
            const string QueryTemplate = "SELECT COUNT(dID) FROM Documents";
            return ExecuteScalarQuery(QueryTemplate.FormatWith(new {SubQuery = docIdsQueryText}));
        }

        public long DocIdCountFromTables()
        {
            const string QueryTemplate = "SELECT COUNT(A) FROM ( {SubQuery} )";
            return ExecuteScalarQuery(QueryTemplate.FormatWith(new {SubQuery = docIdsQueryText}));
        }

        public (IEnumerable<long> notEvenViaParent, IEnumerable<long> onlyViaParent) DocIdOrphansFromDocIndex()
        {
            var commandText = $"SELECT pID,dID FROM Documents WHERE dID NOT IN ( {docIdsQueryText} )";

            var unrefToAdd = new Queue<long>();
            var parentToDoc = new Dictionary<long, List<long>>();


            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = commandText;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(1))
                            continue;
                        var dID = reader.GetInt64(1);
                        var pID = reader.IsDBNull(0)
                            ? null
                            : (long?) reader.GetInt64(0);

                        if (pID == null)
                        {
                            unrefToAdd.Enqueue(dID);
                        }
                        else
                        {
                            if (!parentToDoc.TryGetValue(pID.Value, out var list))
                                list = new List<long>(1);
                            list.Add(dID);
                            parentToDoc[pID.Value] = list;
                        }
                    }
                }
            }

            var notEvenViaParent = new List<long>();

            while (unrefToAdd.Count != 0)
            {
                var toAdd = unrefToAdd.Dequeue();

                notEvenViaParent.Add(toAdd);
                if (!parentToDoc.TryGetValue(toAdd, out var list))
                    continue;


                foreach (var cDoc in list) unrefToAdd.Enqueue(cDoc);

                parentToDoc.Remove(toAdd);
            }

            var onlyViaParent = parentToDoc.SelectMany(p => p.Value);


            return (notEvenViaParent, onlyViaParent);

//            return this.ExecuteScalarQuery(QueryTemplate.FormatWith(new { SubQuery = this.docIdsQueryText }));
        }

        public long DocIdOrphansFromTables()
        {
            const string QueryTemplate = "SELECT COUNT(A) FROM ( {SubQuery} ) WHERE A NOT IN (SELECT dID AS A FROM Documents)";
            return ExecuteScalarQuery(QueryTemplate.FormatWith(new {SubQuery = docIdsQueryText}));
        }

        private long ExecuteScalarQuery(string commandText)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = commandText;
                return Convert.ToInt64(cmd.ExecuteScalar().ToString());
            }
        }

        #endregion
    }
}