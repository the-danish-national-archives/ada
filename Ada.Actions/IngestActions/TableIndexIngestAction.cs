namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ActionBase;
    using Checks;
    using Checks.Table;
    using Checks.TableIndex;
    using ChecksBase;
    using Common;
    using EntityLoaders;
    using Log;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Xml;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("files")]
    [ReportsChecks(
        typeof(TableIndexNotWellFormed),
        typeof(TableIndexInvalid),
        typeof(AdaAvXmlIndexIllegalEncoding),
        typeof(AdaAvXmlIndexMissingProlog),
        typeof(TableIndexTypeOriginalMissing),
        typeof(TableIndexForeignKeyNotUnique), // typeof(TableIndexDuplicateKeys),
        typeof(TableIndexPrimaryKeyNotUnique), // typeof(TableIndexDuplicateKeys),
        typeof(TableNameDuplicate),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps),
        typeof(TableIndexInvalidFolderName),
        typeof(TableIndexDuplicateColumnName),
        typeof(TableIndexDuplicateColumnId),
        typeof(TableIndexSequenceGaps),
        typeof(TablePrimaryKeyIsNullable),
        typeof(TableIndexMissingParentColumns),
        typeof(TableIndexReferencedTableMissing),
        typeof(TableIndexForeignKeyColumnMissingInParent),
        typeof(TableIndexForeignKeyColumnMissingInReferenced),
        typeof(TableIdentifierReservedWords),
        typeof(TableIndexNoRelations)
    )]
    public class TableIndexIngestAction : AdaXmlIngestAction<TableIndexRepo, TableIndex>
    {
        #region  Fields

        private readonly TableContentRepo modelRepo;

        private readonly TableContentRepo ContentRepo; // for model databases

        #endregion

        #region  Constructors

        public TableIndexIngestAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            TableIndexRepo tableIndexRepo,
            TableContentRepo modelRepo,
            TableContentRepo contentRepo)
            : base(processLog,
                testLog,
                GetLogger(mapping),
                mapping,
                tableIndexRepo)
        {
            this.modelRepo = modelRepo;
            ContentRepo = contentRepo;
        }

        #endregion

        #region

        private void CreateDatabase(TableIndex index)
        {
            var sbmodel = new StringBuilder();
            var sbav = new StringBuilder();

            var additionalColumns = new List<string>();

            foreach (var table in index.Tables)
            {
                sbav.Append(GenerateSqlSchema(table, false, additionalColumns));

                sbmodel.Append(GenerateSqlSchema(table, true));
            }

            modelRepo.UpdateSchema(sbmodel.ToString());
            modelRepo.Commit();

            // TableContentRepo.CreateDatabase(this.Mapping.AVID, ".model", sbmodel.ToString(), Properties.Settings.Default.DBCreationFolder);
            ContentRepo.UpdateSchema(sbav.ToString());
            ContentRepo.Commit();
        }

        private static string GenerateSqlSchema
            (Table table, bool withConstraints = false, List<string> AddonColumns = null)
        {
            // TODO: Inject typeconverter?
            const string sqlCreateTableTemplate =
                "DROP TABLE IF EXISTS {TableName}; CREATE TABLE {TableName} ({TableContents});";
            const string sqlConstraintTemplatePk = "CONSTRAINT {ConstraintName} PRIMARY KEY({ConstraintColumns})";
            const string sqlConstraintTemplateFk =
                "CONSTRAINT {ConstraintName} FOREIGN KEY({ConstraintColumns}) REFERENCES {ReferencedTable}({ReferencedColumns})";
            const string sqlColumnTemplate = "{ColumnName} {ColumnType}{NullableConstraint}{DefaultValue}";
            const string notNullLiteral = " NOT NULL";
            const string defaultLiteral = " DEFAULT '{Value}'";

            object formatParams = null;
            var tableContents = new List<string>();

            foreach (var col in table.Columns)
            {
                var nullable = withConstraints && col.Nullable ? notNullLiteral : string.Empty;
                var defaultValue = withConstraints && !string.IsNullOrEmpty(col.DefaultValue)
                    ? defaultLiteral.FormatWith(
                        new {Value = col.DefaultValue.Trim().Replace("\'", "''")})
                    : string.Empty;

                formatParams = new
                {
                    ColumnName = col.Name,
                    ColumnType = "TEXT",
                    NullableConstraint = nullable,
                    DefaultValue = defaultValue
                };
                tableContents.Add(sqlColumnTemplate.FormatWith(formatParams));
            }

            if (AddonColumns != null) tableContents.AddRange(AddonColumns);

            if (withConstraints)
            {
                formatParams = new
                {
                    ConstraintName = table.PrimaryKey.Name,
                    ConstraintColumns = table.PrimaryKey.Columns.SmartToString()
                };

                tableContents.Add(sqlConstraintTemplatePk.FormatWith(formatParams));
                foreach (var key in table.ForeignKeys)
                {
                    formatParams = new
                    {
                        ConstraintName = key.Name,
                        ConstraintColumns = key.References.Select(x => x.Column).SmartToString(),
                        key.ReferencedTable,
                        ReferencedColumns = key.References.Select(x => x.Referenced).SmartToString()
                    };

                    tableContents.Add(sqlConstraintTemplateFk.FormatWith(formatParams));
                }
            }

            return sqlCreateTableTemplate.FormatWith(
                new
                {
                    TableName = table.Name,
                    TableContents = tableContents.SmartToString()
                });
        }

        private static IXmlEventLogger GetLogger(AVMapping localMapping)
        {
            var errorMap =
                new Dictionary<XmlEventType, Type>
                {
                    {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                    {XmlEventType.XmlWellFormednessError, typeof(TableIndexNotWellFormed)},
                    {XmlEventType.XmlValidationError, typeof(TableIndexInvalid)},
                    {XmlEventType.XmlValidationWarning, typeof(TableIndexInvalid)},
                    {XmlEventType.XmlDeclaredEncodingIllegal, typeof(AdaAvXmlIndexIllegalEncoding)},
                    {XmlEventType.XmlMissingProlog, typeof(AdaAvXmlIndexMissingProlog)}
                };

            IXmlEventLogger logger = new XmlEventLogger(
                errorMap,
                localMapping);
            return logger;
        }

        protected override void OnRun(XmlCouplet targetXmlCouplet)
        {
            Logger.CallBack = Report;
            var index = TableIndexloader.Load(
                ArchivalXmlReader,
                targetXmlCouplet.XmlStream,
                targetXmlCouplet.SchemaStream);

            var errors = false;

            errors |= ReportAny(TableIdentifierReservedWords.Check("DataBaseName", index.DbName));

            ReportAny(TableIndexNoRelations.Check(index));

            ReportAny(TableIndexTypeOriginalMissing.Check(index));

            ReportAny(TableIndexDuplicateKeys.Check(index));

            errors |= ReportAny(TableNameDuplicate.Check(index.Tables));


            errors |= ReportAny(TableFolderSequenceInvalidStart.Check(index));

            errors |= ReportAny(TablesFolderSequenceGaps.Check(index));

            if (!errors)
                foreach (var table in index.Tables)
                {
                    errors |= ReportAny(TableIndexInvalidFolderName.Check(table));

                    errors |= ReportAny(TableIndexDuplicateColumnName.Check(table));

                    errors |= ReportAny(TableIndexDuplicateColumnId.Check(table));

                    ReportAny(TableIndexSequenceGaps.Check(table));

                    ReportAny(TablePrimaryKeyIsNullable.Check(table));

                    errors |= ReportAny(TableIdentifierReservedWords.Check("TableName", table.Name));

                    errors |= ReportAny(TableIdentifierReservedWords.Check("ConstraintName", table.PrimaryKey.Name));

                    errors |= ReportAny(TableIndexMissingParentColumns.Check(table));

                    foreach (var col in table.Columns) errors |= ReportAny(TableIdentifierReservedWords.Check("ColumnName", col.Name));

                    foreach (var fk in table.ForeignKeys)
                    {
                        if (ReportAny(TableIndexReferencedTableMissing.Check(fk)))
                            continue;

                        errors |= ReportAny(TableIdentifierReservedWords.Check("ConstraintName", fk.Name));

                        errors |= ReportAny(TableIndexForeignKeyColumnMissingInParent.Check(fk));

                        errors |= ReportAny(TableIndexForeignKeyColumnMissingInReferenced.Check(fk));
                    }
                }

            if (!errors)
                foreach (var view in index.Views)
                    errors |= ReportAny(TableIdentifierReservedWords.Check("ViewName", view.Name));

            if (!errors) CreateDatabase(index);

            TargetRepository.SaveEntity(index);
        }

        #endregion
    }
}