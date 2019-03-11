namespace Ada.ADA.Common.TestActions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    public class PrimaryKeyTest : AdaActionBase<IAdaUowFactory>
    {
        private readonly IAdaUowFactory avFactory;

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var testuow = factory.GetUnitOfWork())
            {
                var pks = testuow.GetRepository<PrimaryKey>().All().ToList();

                foreach (var pk in pks)
                {
                    using (var uow = (UnitOfWork)this.avFactory.GetUnitOfWork())
                    {
                        long distinctRows;
                        long totalRows;
                        long nullRowsCount;
                        long whiteSpaceCount;

                        using (var command = uow.Session.Connection.CreateCommand())
                        {
                            command.CommandText = pk.GetDistinctRowsQuery();
                            distinctRows = (long)command.ExecuteScalar();
                        }

                        using (var command = uow.Session.Connection.CreateCommand())
                        {
                            command.CommandText = pk.GetTotalRowsQuery();
                            totalRows = (long)command.ExecuteScalar();
                        }

                        using (var command = uow.Session.Connection.CreateCommand())
                        {
                            command.CommandText = pk.GetNullRowsCountQuery();
                            nullRowsCount = (long)command.ExecuteScalar();
                        }
                           
                        using (var command = uow.Session.Connection.CreateCommand())
                        {
                            command.CommandText = pk.GetWhiteSpaceRowsCountQuery();
                            whiteSpaceCount = (long)command.ExecuteScalar();
                        }

                        var errorcount = totalRows - distinctRows;
                        var totalErrorCount = errorcount + whiteSpaceCount + nullRowsCount;
                        var errpercentage = (float)totalErrorCount / totalRows * 100;
                        if (totalErrorCount > 0)
                        {
                            IDataReader reader;
                            LogEntry logEntry;
                            long distinctErrorRowsCount = 0;
                            if (errorcount > 0)
                            {
                                using (var command = uow.Session.Connection.CreateCommand())
                                {
                                    command.CommandText = pk.GetDistinctErrorRowsCountQuery();
                                    distinctErrorRowsCount = (long)command.ExecuteScalar();
                                }
                            }

                            if (distinctErrorRowsCount > 0)
                            {
                                using (var command = uow.Session.Connection.CreateCommand())
                                {
                                    command.CommandText = pk.GetDistinctErrorRowsQuery(100);
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        var values = new string[reader.FieldCount];
                                        for (var i = 0; i < reader.FieldCount; i++)
                                        {
                                            values[i] = reader[i].ToString();
                                        }

                                        logEntry = new LogEntry { EntryTypeId = "4.A_1_1" };
                                        logEntry.AddTag("TableName", pk.ParentTableName);
                                        logEntry.AddTag("ConstraintName", pk.Name);
                                        this.ReportLogEntry(logEntry);

                                    }
                                }
                            }

                            if (whiteSpaceCount > 0)
                            {
                                using (var command = uow.Session.Connection.CreateCommand())
                                {
                                    command.CommandText = pk.GetWhiteSpaceRowsQuery(100);
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        var values = new string[reader.FieldCount];
                                        for (var i = 0; i < reader.FieldCount; i++)
                                        {
                                            values[i] = reader[i].ToString();
                                        }

                                        logEntry = new LogEntry { EntryTypeId = "4.A_1_3" };
                                        logEntry.AddTag("TableName", pk.ParentTableName);
                                        logEntry.AddTag("ConstraintName", pk.Name);
                                        this.ReportLogEntry(logEntry);

                                    }
                                }
                            }

                            if (nullRowsCount > 0)
                            {
                                logEntry = new LogEntry { EntryTypeId = "4.A_1_2" };
                                logEntry.AddTag("TableName", pk.ParentTableName);
                                logEntry.AddTag("Count", nullRowsCount.ToString());
                                logEntry.AddTag("ConstraintName", pk.Name);
                                this.ReportLogEntry(logEntry);

                            }

                            logEntry = new LogEntry { EntryTypeId = "4.A_1_4" };
                            logEntry.AddTag("TableName", pk.ParentTableName);
                            logEntry.AddTag("ConstraintName", pk.Name);
                            logEntry.AddTag("TotalErrors", totalErrorCount.ToString());
                            logEntry.AddTag("ErrorCount", errorcount.ToString());
                            logEntry.AddTag("DistinctErrorCount", distinctErrorRowsCount.ToString());
                            logEntry.AddTag("NullErrorCount", nullRowsCount.ToString());
                            logEntry.AddTag("WhiteSpaceCount", whiteSpaceCount.ToString());
                            logEntry.AddTag("TotalRows", totalRows.ToString());
                            logEntry.AddTag("ErrorPercentage", errpercentage.ToString());

                            this.ReportLogEntry(logEntry);

                        }
                    }
                }
            }
        }

        public PrimaryKeyTest(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory avFactory, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.avFactory = avFactory;
        }
    }
}