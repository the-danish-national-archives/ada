namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using ActionBase;
    using ChecksBase;
    using Common;
    using Log;
    using Ra.Common.Repository.NHibernate;
    using Repositories;

    #endregion

//    [AdaActionPrecondition("indexSchemas", "standardSchemasOnDisk")]
    public class SchemaVersionTestAction : AdaActionBase<string>
    {
        #region  Fields

        private readonly Type _schemaVersionError;
        private readonly IAdaUowFactory testFactory;

        #endregion

        #region  Constructors

        public SchemaVersionTestAction(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping, Type schemaVersionError)
            : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
            _schemaVersionError = schemaVersionError;
        }

        #endregion

        #region

        protected override void OnRun(string schemaName)
        {
            var expectedDigest = string.Empty;
            var actualDigest = string.Empty;
            var name = string.Empty;
            var version = string.Empty;

            using (var uow = (UnitOfWork) testFactory.GetUnitOfWork())
            {
                var comm = uow.Session.Connection.CreateCommand();

                comm.CommandText =
                    @"SELECT indexSchemas.name AS name, indexSchemas.md5 AS expected, 
        standardSchemasOnDisk.checksum AS actual, version FROM indexSchemas 
    INNER JOIN standardSchemasOnDisk ON standardSchemasOnDisk.Name = indexSchemas.Name WHERE standardSchemasOnDisk.Name='" +
                    schemaName + ".xsd'";

                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                    version = reader["version"].ToString();
                    expectedDigest = reader["expected"].ToString().ToUpper();
                    actualDigest = reader["actual"].ToString().ToUpper();
                    //Nope!
                }
            }

            if (string.IsNullOrEmpty(actualDigest))
                using (var repo = new AdaStructureRepo(
                    testFactory,
                    0))
                {
                    var file = repo.GetStandardSchema(schemaName);
                    var md5 = MD5.Create();
                    var path = Path.Combine(
                        Mapping.GetMediaRoot(1),
                        file.RelativePath,
                        file.Name);
                    if (File.Exists(path))
                        using (var stream = File.OpenRead(path))
                        {
                            var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                            actualDigest = hash;
                        }
                }

            if (expectedDigest != actualDigest)
            {
                Report(AdaAvSchemaVersion.CreateInstance(_schemaVersionError, name, version));

                throw new AdaSkipAllActionException();
            }
        }

        #endregion
    }
}