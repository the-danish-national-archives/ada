namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using IngestActions;
    using Log;
    using Repositories;
    using TestActions;

    #endregion

    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(IndicesTableIndex), // '4.C_4'
        typeof(FileIndexNotWellFormed), // '4.C.1_1'
        typeof(FileIndexInvalid), // '4.C.1_2'
        typeof(TableIndexNotWellFormed), // '4.C.1_7'
        typeof(TableIndexInvalid), // '4.C.1_8'
        typeof(TableIdentifierReservedWords),
        typeof(TableNameDuplicate), // '4.C.5_3'
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps),
        typeof(SchemaMissingFolder), // '4.F_1'
        typeof(AdaAvSchemaVersionTableIndex), // '4.F_5'
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema), // '4.F_8'
        typeof(TableIndexDuplicateColumnName), // '6.C_2'
        typeof(TableIndexDuplicateColumnId), // '6.C_3'
        typeof(TableIndexReferencedTableMissing), // '6.C_19'
        typeof(TableIndexMissingParentColumns), // '6.C_20'
        typeof(TableIndexForeignKeyColumnMissingInReferenced), // '6.C_21'
        typeof(TableIndexForeignKeyColumnMissingInParent),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed)
    )]
    [RunsActions(
        typeof(TableIngestPreCheckAction),
        typeof(TableIngestAction),
        typeof(DateTimeOuterLimitsTestAction)
    )]
    public class TableIngestPresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Fields

        private readonly IAdaUowFactory _contentUowFactory;

        #endregion

        #region  Constructors

        public TableIngestPresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaUowFactory contentUowFactory)
            : base(processLog, testLog, mapping)
        {
            _contentUowFactory = contentUowFactory;
        }

        #endregion

        #region

        protected override void OnRun(AdaUowTarget testSubject)
        {
            using (var adaStructureRepo = new AdaStructureRepo(testSubject.AdaUowFactory, 1000))
            {
                new TableIngestPreCheckAction(GetSubordinateProcessLog(), GetAdaTestLog(), Mapping, adaStructureRepo).Run(
                    testSubject.AdaUowFactory);
            }

            var tableIngestAction = new TableIngestAction(GetSubordinateProcessLog(), GetAdaTestLog(), Mapping, testSubject.AdaUowFactory);
            tableIngestAction.ProgressCallback = ProgressCallback;
            tableIngestAction.Run(
                new TableIngestAction.TableIngestActionTarget(_contentUowFactory));

            using (var tableContentRepo = new TableContentRepo(_contentUowFactory, 1000))
            {
                new DateTimeOuterLimitsTestAction(
                    GetSubordinateProcessLog(),
                    GetAdaTestLog(),
                    Mapping,
                    tableContentRepo).Run(testSubject.AdaUowFactory);
            }
        }

        #endregion
    }
}