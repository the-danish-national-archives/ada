namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using System.Linq;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.PrimaryKey;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using Log;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("TableIndex")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(IndicesTableIndex),
        typeof(FileIndexNotWellFormed), // '4.C.1_1'
        typeof(FileIndexInvalid), // '4.C.1_2'
        typeof(TableIndexNotWellFormed), // '4.C.1_7'
        typeof(TableIndexInvalid), // '4.C.1_8'
        typeof(TableNameDuplicate), // '4.C.5_3'
        typeof(TableNotWellFormed), // '4.D_8'
        typeof(TableNotValid), // '4.D_9'
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
        typeof(TableIdentifierReservedWords),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(
        typeof(PrimaryKeyDuplikates),
        typeof(PrimaryKeyBlanks),
        typeof(PrimaryKeyNull)
    )]
    public class PrimaryKeyTestAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        private readonly IAdaUowFactory avFactory;

        #endregion

        #region  Constructors

        public PrimaryKeyTestAction
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaUowFactory avFactory)
            : base(processLog, testLog, mapping)
        {
            this.avFactory = avFactory;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var testuow = factory.GetUnitOfWork())
            {
                var pks = testuow.GetRepository<PrimaryKey>().All().ToList();

                foreach (var pk in pks)
                    using (var sqlRepo = new SimpleAvSqlRepo(avFactory))
                    {
                        ReportAny(
                            PrimaryKeyDuplikates.Check(
                                pk,
                                sqlRepo));

                        ReportAny(PrimaryKeyNull.Check(pk, sqlRepo));

                        ReportAny(PrimaryKeyBlanks.Check(pk, sqlRepo));
                    }
            }
        }

        #endregion
    }
}