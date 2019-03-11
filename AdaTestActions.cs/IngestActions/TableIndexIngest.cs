namespace Ada.Common.IngestActions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    using global::Ada.ADA.EntityLoaders;
    using global::Ada.Log;
    using global::Ada.Log.Entities;
    using global::Ada.Repositories;

    using Ra.Common.Xml;
    using Ra.Common;
    using Ra.DomainEntities.TableIndex.Extensions;
    using Ra.EntityExtensions.TableIndex;


    public class TableIndexIngest : AdaXmlIngest
    {
        private readonly IAdaUowFactory testFactory;

        public TableIndexIngest(
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            IAdaUowFactory testFactory,
            IArchivalXmlReader reader,
            IXmlEventLogger logger,
            AVMapping mapping)
            : base(processLog, testLog,reader, logger, mapping)
        {
            this.testFactory = testFactory;
        }

        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            var index = TableIndexloader.Load(this.ArchivalXmlReader, targetXmlCouplet.XmlStream, targetXmlCouplet.SchemaStream);

           
            if (!string.IsNullOrEmpty(index.DbProduct))
            {
                var missingTypes = (from table in index.Tables from column in table.Columns select column.TypeOriginal).Count(string.IsNullOrEmpty);
                if (missingTypes > 0)
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.C_17" };
                    logEntry.AddTag("Count", missingTypes.ToString());
                    this.ReportLogEntry(logEntry);
                }


            }

            var duplicateTableNames = index.Tables.GroupBy(x => x.Name).Where(g => g.Count() > 1).Select(y => new { Name = y.Key, Count = y.Count() }).ToList();

            var allConstraints = new List<Constraint>();
             
            foreach (var table in index.Tables)
            {
                allConstraints.Add(table.PrimaryKey);
                allConstraints.AddRange(table.ForeignKeys);
            }

            var duplicateConstraints = allConstraints.GroupBy(x => x.Name).Where(g => g.Count() > 1).Select(x => new { Name = x.Key, Count = x.Count(), Items = x.ToList() }).ToList();
            var t = duplicateConstraints.Count();

            foreach (var duplicateGroup in duplicateConstraints)
            {
                foreach (var duplicate in duplicateGroup.Items)
                {
                    if (duplicate is ForeignKey)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "6.C_7" };
                        logEntry.AddTag("ConstraintName", duplicate.Name);
                        logEntry.AddTag("TableName", duplicate.ParentTableName);
                        logEntry.AddTag("FolderName", duplicate.ParentTable.Folder);
                        this.ReportLogEntry(logEntry);                        
                    }

                    if (duplicate is PrimaryKey)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "6.C_6" }; 
                        logEntry.AddTag("ConstraintName", duplicate.Name);
                        logEntry.AddTag("TableName", duplicate.ParentTableName);
                        logEntry.AddTag("FolderName", duplicate.ParentTable.Folder);
                        this.ReportLogEntry(logEntry);      
                    }
                }                      
            }



            var errors = false;

            foreach (var duplicate in duplicateTableNames)
            {
                var logEntry = new LogEntry { EntryTypeId = "4.C.5_3" };
                logEntry.AddTag("TableName", duplicate.Name);
                logEntry.AddTag("Count", duplicate.Count.ToString(CultureInfo.InvariantCulture));
                this.ReportLogEntry(logEntry);
            }

            if (!index.FolderSequenceValidStart())
            {
                var logEntry = new LogEntry { EntryTypeId = "4.D_4" };
                this.ReportLogEntry(logEntry);
            }

            if (index.FolderSequenceGaps().Any())
            {
                var logEntry = new LogEntry { EntryTypeId = "4.D_3" };
                logEntry.AddTag("MissingTables", index.FolderSequenceGaps().SmartToString());
                this.ReportLogEntry(logEntry);
            }


            foreach (var table in index.Tables)
            {
                if (!table.FolderNameIsValid())
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.C_27" };
                    logEntry.AddTag("TableName", table.Name);
                    logEntry.AddTag("FolderName", table.Folder);
                    this.ReportLogEntry(logEntry);
                    errors = true;
                }


                foreach (var duplicate in table.DuplicateColumnNames())
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.C_2" };
                    logEntry.AddTag("TableName", table.Name);
                    logEntry.AddTag("ColumnName", duplicate.Key);
                    logEntry.AddTag("Count", duplicate.Value.ToString(CultureInfo.InvariantCulture));
                    this.ReportLogEntry(logEntry);
                    errors = true;
                }

                foreach (var duplicate in table.DuplicateColumnIds())
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.C_3" };
                    logEntry.AddTag("TableName", table.Name);
                    logEntry.AddTag("ColumnID", duplicate.Key);
                    logEntry.AddTag("Count", duplicate.Value.ToString(CultureInfo.InvariantCulture));
                    this.ReportLogEntry(logEntry);
                    errors = true;
                }

                if (table.SequenceGaps().Any())
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.C_4"};
                    logEntry.AddTag("TableName", table.Name);
                    logEntry.AddTag("FolderNumber", table.Folder);
                    this.ReportLogEntry(logEntry);
                }

                foreach (var nullablePkCol in table.PrimaryKey.IsNullable())
                {
                    var logEntry = new LogEntry { EntryTypeId = "4.C.5_2" };
                    logEntry.AddTag("ColumnName", nullablePkCol.Name);
                    logEntry.AddTag("ColumnNumber", nullablePkCol.ColumnId);
                    logEntry.AddTag("TableName", table.Name);
                    logEntry.AddTag("FolderNumber", table.Folder);
                    this.ReportLogEntry(logEntry);
                }



                foreach (var col in table.PrimaryKey.GetMissingParentColumns())
                {
                    var logEntry = new LogEntry { EntryTypeId = "6.C_20" };
                    logEntry.AddTag("ConstraintName", table.PrimaryKey.Name);
                    logEntry.AddTag("TableName", table.Name);
                    logEntry.AddTag("ColumnName", col);
                    this.ReportLogEntry(logEntry);
                    errors = true;
                }




                foreach (var fk in table.ForeignKeys)
                {
                    if (fk.GetReferencedTable() == null)
                    {
                        var logEntry = new LogEntry { EntryTypeId = "6.C_19" };
                        logEntry.AddTag("ConstraintName", fk.Name);
                        logEntry.AddTag("TableName", table.Name);
                        logEntry.AddTag("ReferencedTable", fk.ReferencedTable);
                        this.ReportLogEntry(logEntry);
                        errors = true;
                    }
                    else
                    {
                        foreach (var col in fk.GetMissingParentColumns())
                        {
                            var logEntry = new LogEntry { EntryTypeId = "6.C_22" };
                            logEntry.AddTag("ConstraintName", fk.Name);
                            logEntry.AddTag("TableName", table.Name);
                            logEntry.AddTag("ColumnName", col);
                            this.ReportLogEntry(logEntry);
                            errors = true;
                        }

                        foreach (var col in fk.GetMissingReferencedColumns())
                        {
                            var logEntry = new LogEntry { EntryTypeId = "6.C_21" };
                            logEntry.AddTag("ConstraintName", fk.Name);
                            logEntry.AddTag("TableName", table.Name);
                            logEntry.AddTag("ReferencedTableName", fk.ReferencedTable);
                            logEntry.AddTag("ColumnName", col);
                            this.ReportLogEntry(logEntry);
                            errors = true;
                        }
                    }
                } 
            }
            if (!duplicateTableNames.Any() && !errors)
            {

                var sbmodel = new StringBuilder();
                var sbav = new StringBuilder();

                var additionalColumns = new List<string>()
                                        {
                                            "tableRecordCounter TEXT",
                                            "binaryPosStart TEXT",
                                            "binaryPosEnd TEXT"
                                        };


                foreach (var table in index.Tables)
                {
                    sbav.Append(table.GetSqlSchema(false, additionalColumns));
                    sbmodel.Append(table.GetSqlSchema(true));
                }

                TableContentRepo.CreateDatabase(this.Mapping.AVID, ".model", sbmodel.ToString(), Properties.Settings.Default.DBCreationFolder);
                TableContentRepo.CreateDatabase(this.Mapping.AVID, ".av", sbav.ToString(), Properties.Settings.Default.DBCreationFolder); 
            }


            using (var uow = testFactory.GetUnitOfWork())
            {

                uow.BeginTransaction();
                var repo = uow.GetRepository<TableIndex>();
                repo.Add(index);
                uow.Commit();
            }
        }



      
    }
}