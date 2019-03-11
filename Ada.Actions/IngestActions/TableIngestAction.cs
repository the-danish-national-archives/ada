namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ActionBase;
    using Checks.Table;
    using ChecksBase;
    using Common;
    using Log;
    using Ra.Common;
    using Ra.Common.Repository.NHibernate;
    using Ra.Common.Xml;
    using Ra.DomainEntities.FileIndex;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.FileIndex;
    using Ra.EntityExtensions.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("tableIndex", "files")]
    [RequiredChecks(
        typeof(TableIdentifierReservedWords),
        typeof(TableNameDuplicate),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(
        typeof(TableNotWellFormed),
        typeof(TableNotValid),
        typeof(TableMissingProlog),
        typeof(TableIllegalEncoding),
        typeof(TableRowCountMismatch),
        typeof(TableLeadingOrTrailingWhitespace),
        typeof(TableFieldLengthMismatch),
        typeof(TableFieldLagringsform),
        typeof(DocumentsCountingProblemFiles)
    )]
    public class TableIngestAction : AdaActionBase<TableIngestAction.TableIngestActionTarget>
    {
        #region  Fields

        protected readonly IAdaUowFactory testFactory;

        #endregion

        #region  Constructors

        public TableIngestAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            IAdaUowFactory testFactory)
            : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
        }

        public TableIngestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(TableIngestActionTarget target)
        {
            var adaSkipSeen = false;
            using (var uow = (UnitOfWork) testFactory.GetUnitOfWork())
            {
                var documentsCountingProblemFilesSummery =
                    new DocumentsCountingProblemFiles.DocumentsCountingProblemFilesSummery();


                var tablesList = uow.GetRepository<Table>().All().ToList();

                var totalDoc = tablesList.Count;
                var docI = 0;

                void Reporter()
                {
                    ++docI;
                    ProgressCallback?.Invoke($"{docI} ud af {totalDoc}");
                }


                foreach (var table in tablesList)
                {
                    try
                    {
                        GC.Collect(1);

                        var errorMap =
                            new Dictionary<XmlEventType, Type>
                            {
                                {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                                {XmlEventType.XmlWellFormednessError, typeof(TableNotWellFormed)},
                                {XmlEventType.XmlValidationError, typeof(TableNotValid)},
                                {XmlEventType.XmlValidationWarning, typeof(TableNotValid)},
                                {XmlEventType.XmlMissingProlog, typeof(TableMissingProlog)},
                                {XmlEventType.XmlDeclaredEncodingIllegal, typeof(TableIllegalEncoding)}
                            };

                        IXmlEventLogger logger = new XmlEventLogger(
                            errorMap,
                            Mapping);
                        logger.CallBack = Report;
                        var reader = new ArchivalXmlReader(new XmlEventFilter());
                        reader.XmlEvent += logger.EventHandler;

                        var fileRepo = uow.GetRepository<FileIndexEntry>();
                        var tableMetaData =
                            fileRepo.All().FirstOrDefault(x => x.Extension == ".xml" && x.FileName == table.Folder);
                        if (tableMetaData == null)
                            continue;

                        var path =
                            Mapping.GetMediaPath(int.Parse(tableMetaData.MediaNumber)) +
                            tableMetaData.RelativePathAndFile();

                        var tableXmlStream = new BufferedProgressStream(new FileInfo(path));
                        var tableXsdStream = new BufferedProgressStream(table.GetXmlSchemaStream());
                        long rowCount = 0;

                        using (var tableRepo = new TableContentRepo(target.Factory, 1000))
                        {
                            var tableLeadingOrTrailingWhitespaceSummery =
                                new TableLeadingOrTrailingWhitespace.TableLeadingOrTrailingWhitespaceSummery(table);
                            var tableFieldLengthMismatchSummery =
                                new TableFieldLengthMismatch.TableFieldLengthMismatchSummery(table);
                            var tableFieldLagringsformSummery =
                                new TableFieldLagringsform.TableFieldLagringsformSummery(table);
                            var problemFileCheckFunc = documentsCountingProblemFilesSummery.GetCheckFunc(table);


                            var insertTemplate = TableContentRepo.GetInsertTemplate(table);
                            reader.Open(tableXmlStream, tableXsdStream);
                            foreach (var rowElement in reader.ElementStream("row"))
                            {
                                var rowContents = new SortedList<string, string>();
                                foreach (var columnElement in rowElement.Elements())
                                    rowContents.Add(
                                        columnElement.Name.LocalName,
                                        columnElement.Attribute("{http://www.w3.org/2001/XMLSchema-instance}nil")
                                        == null
                                            ? columnElement.Value.Replace("\'", "''")
                                            : null
                                    );

                                rowCount++;
                                tableLeadingOrTrailingWhitespaceSummery.Check(rowContents, rowCount);
                                tableFieldLengthMismatchSummery.Check(rowContents);
                                tableFieldLagringsformSummery.Check(rowContents);
                                problemFileCheckFunc?.Invoke(rowContents);

                                tableRepo.AddRow(insertTemplate, rowContents);
                            }

                            ReportAny(tableLeadingOrTrailingWhitespaceSummery.Report());
                            ReportAny(tableFieldLengthMismatchSummery.Report());
                            ReportAny(tableFieldLagringsformSummery.Report());

                            ReportAny(TableRowCountMismatch.Check(table.Name, Convert.ToInt64(table.Rows), rowCount));
                            reader.Close();
                            tableRepo.Commit();
                        }
                    }
                    catch (AdaSkipActionException e)
                    {
                        adaSkipSeen = true;
                    }

                    Reporter();
                }

                ReportAny(documentsCountingProblemFilesSummery.Report());
            }

            if (adaSkipSeen)
                throw new AdaSkipAllActionException("A skip message seen while loading tables");
        }

        #endregion

        #region Nested type: TableIngestActionTarget

        public class TableIngestActionTarget
        {
            #region  Constructors

            public TableIngestActionTarget(IAdaUowFactory factory)
            {
                Factory = factory;
            }

            #endregion

            #region Properties

            public IAdaUowFactory Factory { get; set; }

            #endregion
        }

        #endregion
    }
}