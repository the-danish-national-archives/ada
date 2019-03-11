using System;
using System.Data;

using Ra.Common.Repository.NHibernate;

namespace Ada.Common.TestActions
{
    using Ada.ADA.Common;
    using Log;
    using Ada.Log.Entities;
    using Ada.Repositories;

    public class TestSchemaVersion : AdaActionBase<string>
    {
        private readonly IAdaUowFactory testFactory;

       // private readonly string _schemaName;
        private readonly string _errorCode;

        public TestSchemaVersion(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping, string errorCode)
            : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
            //_schemaName = schemaName;
            _errorCode = errorCode;
        }

        protected override void OnRun(string schemaName)
        {
            string expectedDigest = string.Empty;
            string actualDigest = string.Empty;
            string name = string.Empty;
            string version = string.Empty;

            using (UnitOfWork uow = (UnitOfWork)this.testFactory.GetUnitOfWork())
            {
                IDbCommand comm = uow.Session.Connection.CreateCommand();

                comm.CommandText =
                    "select name, indexSchemas.md5 as expected, filesOnDisk.MD5 as actual, version from indexSchemas INNER JOIN filesOnDisk ON (fileName || extension) = name WHERE fileName='" +
                    schemaName + "'";

                IDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                    version = reader["version"].ToString();
                    expectedDigest = reader["expected"].ToString().ToUpper();
                    actualDigest = reader["actual"].ToString().ToUpper();
                    //Nope!
                }
                if (expectedDigest != actualDigest && !String.IsNullOrEmpty(actualDigest))
                {
                    LogEntry logEntry = new LogEntry();
                    logEntry.EntryTypeId = _errorCode;
                    logEntry.AddTag(LogEntryTagType.FileName, name);
                    logEntry.AddTag(LogEntryTagType.Version, version);

                    this.ReportLogEntry(logEntry);
                    throw new AdaSkipAllActionException();
                }
            }
        }
    }
}