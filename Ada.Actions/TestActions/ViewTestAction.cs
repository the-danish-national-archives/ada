namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using Log;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("tableIndex")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(IndicesArchiveIndex), // '4.C_2'
        typeof(IndicesContextDocIndex), // '4.C_3'
        typeof(IndicesTableIndex), // '4.C_4'
        typeof(FileIndexNotWellFormed), // '4.C.1_1'
        typeof(FileIndexInvalid), // '4.C.1_2'
        typeof(TableIndexNotWellFormed), // '4.C.1_7'
        typeof(TableIndexInvalid), // '4.C.1_8'
        typeof(TableNameDuplicate), // '4.C.5_3'
        typeof(SchemaMissingFolder), // '4.F_1'
        typeof(AdaAvSchemaVersionTableIndex), // '4.F_5'
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema), // '4.F_8'
        typeof(TableIndexDuplicateColumnName), // '6.C_2'
        typeof(TableIndexDuplicateColumnId), // '6.C_3'
        typeof(TableIndexReferencedTableMissing), // '6.C_19'
        typeof(TableIndexMissingParentColumns), // '6.C_20'
        typeof(TableIndexForeignKeyColumnMissingInReferenced), // '6.C_21'
        typeof(TableIndexForeignKeyColumnMissingInParent), // '6.C_22'
        typeof(TableIdentifierReservedWords),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(typeof(TableIndexNotifyViews),
        typeof(TableIndexDuplikateViewName),
        typeof(TableIndexViewsMissingDescription),
        typeof(TableIndexViewsUnwantedContent),
        typeof(TableIndexViewSqlExecuted))]
    public class ViewTestAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        private readonly IAdaUowFactory avFactory;

        #endregion

        #region  Constructors

        public ViewTestAction
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaUowFactory avFactory)
            : base(processLog, testLog, mapping)
        {
            this.avFactory = avFactory;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            List<View> views;

            using (var uow = (UnitOfWork) factory.GetUnitOfWork())
            {
                views = uow.GetRepository<View>().All().ToList();
            }

            using (var uow = (UnitOfWork) avFactory.GetUnitOfWork())
            {
                ReportAny(TableIndexNotifyViews.Create(views));

                ReportAny(TableIndexDuplikateViewName.Check(views));

                foreach (var view in views.Where(v => v.IsAvView()))
                {
                    ReportAny(TableIndexViewsMissingDescription.Check(view));

                    if (ReportAny(TableIndexViewsUnwantedContent.Check(view)))
                        continue;

                    ReportAny(TableIndexViewSqlExecuted.Check(uow, view));
                }
            }
        }

        #endregion
    }
}