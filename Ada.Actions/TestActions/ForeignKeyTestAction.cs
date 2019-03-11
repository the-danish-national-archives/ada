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
    using Checks.ForeignKey;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using Log;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("Tables")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // canRunQuery = "select COUNT(*) FROM logEntries WHERE entrytypeid = '0.3' " // 
        typeof(FolderStructureFirstMediaMissing), // + "OR entrytypeid = '4.B.1_1' " // 
        typeof(FolderStructureDuplicateMediaNumber), // + "OR entrytypeid = '4.B.1_2' " // 
        typeof(FolderStructureMediaNumberGaps), // + "OR entrytypeid = '4.B.1_3' " // 
        typeof(FolderStructureMissingIndicesFirstMedia), // + "OR entrytypeid = '4.B.2_1' " // 
        typeof(FolderStructureSchemasMissing), // + "OR entrytypeid = '4.B.2_2' " // 
        typeof(FolderStructureTablesMissing), // + "OR entrytypeid = '4.B.2_4' " // 
        typeof(IndicesTableIndex),
        typeof(IndicesFileIndex), // + "OR entrytypeid = '4.C_1' " // 
        typeof(FileIndexNotWellFormed), // + "OR entrytypeid = '4.C.1_1' " // 
        typeof(FileIndexInvalid), // + "OR entrytypeid = '4.C.1_2' " // 
        typeof(TableIndexNotWellFormed), // + "OR entrytypeid = '4.C.1_7' " // 
        typeof(TableIndexInvalid), // + "OR entrytypeid = '4.C.1_8' " // 
        typeof(TableNameDuplicate), // + "OR entrytypeid = '4.C.5_3' " // 
        typeof(TableNotWellFormed), // + "OR entrytypeid = '4.D_8' " // 
        typeof(TableNotValid), // + "OR entrytypeid = '4.D_9' " // 
        typeof(SchemaMissingFolder), // + "OR entrytypeid = '4.F_1' " // 
        typeof(AdaAvSchemaVersionTableIndex), // + "OR entrytypeid = '4.F_5' " // 
        typeof(AdaAvSchemaVersionFileIndex), // + "OR entrytypeid = '4.F_6' " // 
        typeof(AdaAvSchemaVersionXmlSchema), // + "OR entrytypeid = '4.F_8' " // 
        typeof(TableIndexDuplicateColumnName), // + "OR entrytypeid = '6.C_2' " // 
        typeof(TableIndexDuplicateColumnId), // + "OR entrytypeid = '6.C_3' " // 
        typeof(TableIndexReferencedTableMissing), // + "OR entrytypeid = '6.C_19' " // 
        typeof(TableIndexMissingParentColumns), // + "OR entrytypeid = '6.C_20' " // 
        typeof(TableIndexForeignKeyColumnMissingInReferenced),

// + "OR entrytypeid = '6.C_21' " // 
        typeof(TableIndexForeignKeyColumnMissingInParent),
        typeof(TableIdentifierReservedWords),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(
        typeof(ForeignKeyReferenceNotPrimary),
        typeof(ForeignKeyMissingPrimaryColumns),
        typeof(ForeignKeyTypeMismatch),
        typeof(ForeignKeyNullCount),
        typeof(ForeignKeyWithNoTarget),
        typeof(ForeignKeyBlanks)
    )]
    public class ForeignKeyTestAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        private readonly IAdaUowFactory avFactory;

        #endregion

        #region  Constructors

        public ForeignKeyTestAction
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaUowFactory avFactory)
            : base(processLog, testLog, mapping)
        {
            this.avFactory = avFactory;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var testuow = (UnitOfWork) factory.GetUnitOfWork())
            {
                var fks = testuow.GetRepository<ForeignKey>().All().ToList();

                foreach (var fk in fks)
                {
                    var fkError = false;

                    fkError |= ReportAny(ForeignKeyReferenceNotPrimary.Check(fk));

                    fkError |= ReportAny(ForeignKeyMissingPrimaryColumns.Check(fk));

                    fkError |= ReportAny(ForeignKeyTypeMismatch.Check(fk));

                    if (fkError)
                        continue;

                    using (var sqlRepo = new SimpleAvSqlRepo(avFactory))
                    {
                        ReportAny(ForeignKeyNullCount.Check(fk, sqlRepo));

                        ReportAny(ForeignKeyWithNoTarget.Check(fk, sqlRepo));

                        ReportAny(ForeignKeyBlanks.Check(fk, sqlRepo));
                    }
                }
            }
        }

        #endregion
    }
}