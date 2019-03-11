using System.Collections.Generic;
using System.Linq;

namespace Ada.ADA.Common.TestActions
{
    using System.Globalization;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.ExtensionMethods;

    using Ra.DomainEntities.TableIndex;
    using Ra.DomainEntities.TableIndex.Extensions;
    using Ra.EntityExtensions.TableIndex;

    using Ra.Common.Repository.NHibernate;

    public class ForeignKeyTest : AdaActionBase<IAdaUowFactory>
    {
    
        private readonly IAdaUowFactory avFactory;

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var testuow = (UnitOfWork)factory.GetUnitOfWork())
            {
                var fks = testuow.GetRepository<ForeignKey>().All().ToList();
               
                foreach (var fk in fks)
                {
                    var fkcorrect = true;

                    var nonRefs = fk.GetNonPrimaryReferences();
                    foreach (var nonRef in nonRefs)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "4.A_1_7" };
                        logEntry.AddTag("TableName", fk.ParentTableName);
                        logEntry.AddTag("ReferencedTable", fk.ReferencedTable);
                        logEntry.AddTag("ConstraintName", fk.Name);
                        logEntry.AddTag("Column", nonRef.Referenced);
                        this.ReportLogEntry(logEntry);

                        fkcorrect = false;
                    }

                    var missingRefs = fk.GetMissingPrimaryColumns();
                    foreach (var missingRef in missingRefs)
                    {
                        var entry = new LogEntry { EntryTypeId = "4.A_1_6" };
                        entry.AddTag("TableName", fk.ParentTableName);
                        entry.AddTag("ReferencedTable", fk.ReferencedTable);
                        entry.AddTag("ConstraintName", fk.Name);
                        entry.AddTag("Column", missingRef);
                        this.ReportLogEntry(entry);

                        fkcorrect = false;
                    }

                    foreach (var reference in fk.References)
                    {
                        var colType = reference.ConstraintFieldType();
                        var refType = reference.ReferencedFieldType();
                        if (!colType.Equals(refType))
                        {
                            var logEntry = new LogEntry { EntryTypeId = "4.A_1_8" };
                            logEntry.AddTag("ConstraintName", fk.Name);
                            logEntry.AddTag("TableName", fk.ParentTableName);
                            logEntry.AddTag("ReferencedTable", fk.ReferencedTable);
                            logEntry.AddTag("Column", reference.Column); 
                            logEntry.AddTag("ReferencedColumn", reference.Referenced);
                            logEntry.AddTag("ColumnType", colType);
                            logEntry.AddTag("ReferencedColumnType", refType);

                            this.ReportLogEntry(logEntry);

                            fkcorrect = false;             
                        }
                    }

                    if (fkcorrect)
                    {
                        using (var uow = (UnitOfWork)avFactory.GetUnitOfWork())
                        {
                            long rowcount = 0;
                            long matchcount = 0;
                            long distinctCount = 0;
                            long emptyfkcount = 0;

                            using (var command = uow.Session.Connection.CreateCommand())
                            {
                                command.CommandText = fk.GetDistinctErrorRowsCountQuery();
                                distinctCount = (long)command.ExecuteScalar();
                            }

                            using (var command = uow.Session.Connection.CreateCommand())
                            {
                                command.CommandText = fk.GetActualMatchesQuery();
                                matchcount = (long)command.ExecuteScalar();
                            }

                            using (var command = uow.Session.Connection.CreateCommand())
                            {
                                command.CommandText = fk.GetTotalRowsQuery();
                                rowcount = (long)command.ExecuteScalar();
                            }

                            using (var command = uow.Session.Connection.CreateCommand())
                            {
                                command.CommandText = fk.GetNullRowsCountQuery();
                                emptyfkcount = (long)command.ExecuteScalar();
                            }

                            long errorcount = rowcount - matchcount - emptyfkcount;
                            var errpercentage = (rowcount > 0) ? (decimal)errorcount / rowcount * 100 : 0;



                            if (emptyfkcount > 0)
                            {
                                var emptypercentage = (rowcount > 0) ? (decimal)emptyfkcount / rowcount * 100 : 0;
                                var logEntry = new LogEntry { EntryTypeId = "4.A_1_11" };
                                logEntry.AddTag("TableName", fk.ParentTableName);
                                logEntry.AddTag("ConstraintName", fk.Name);
                                logEntry.AddTag("Count", emptyfkcount.ToString(CultureInfo.InvariantCulture));
                                logEntry.AddTag("RowCount", rowcount.ToString(CultureInfo.InvariantCulture));
                                logEntry.AddTag("Percentage", emptypercentage.ToString("0.00", CultureInfo.InvariantCulture));

                                this.ReportLogEntry(logEntry);
                            }

                            if (errorcount > 0)
                            {
                                var logEntry = new LogEntry { EntryTypeId = "4.A_1_9" };
                                logEntry.AddTag("ErrorPercentage", errpercentage.ToString("0.00", CultureInfo.InvariantCulture));

                                logEntry.AddTag("ConstraintName", fk.Name);
                                logEntry.AddTag("TableName", fk.ParentTableName);
                                logEntry.AddTag("TotalErrors", errorcount.ToString(CultureInfo.InvariantCulture));
                                logEntry.AddTag("DistinctErrors", distinctCount.ToString(CultureInfo.InvariantCulture));
                                logEntry.AddTag("RowCount", rowcount.ToString(CultureInfo.InvariantCulture));

                                this.ReportLogEntry(logEntry);

                            }


                            if (distinctCount > 0)
                            {
                                using (var command = uow.Session.Connection.CreateCommand())
                                {
                                    command.CommandText = fk.GetDistinctErrorRowsQuery(10);
                                    var reader = command.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        var values = new string[reader.FieldCount];
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            values[i] = reader[i].ToString();
                                        }

                                        var logEntry = new LogEntry { EntryTypeId = "4.A_1_5" };
                                        logEntry.AddTag("ConstraintName", fk.Name);
                                        logEntry.AddTag("TableName", fk.ParentTableName);
                                        logEntry.AddTag("ReferencedTable", fk.ReferencedTable);
                                        logEntry.AddTag("Columns",fk.ConstraintColumns.Select(x => x.Column).SmartToString());
                                        logEntry.AddTag("ReferencedColumns",fk.References.Select(x => x.Referenced).SmartToString());
                                        logEntry.AddTag("Values", values.SmartToString());

                                        this.ReportLogEntry(logEntry);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public ForeignKeyTest(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory avFactory, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.avFactory = avFactory;
        }
    }
}

