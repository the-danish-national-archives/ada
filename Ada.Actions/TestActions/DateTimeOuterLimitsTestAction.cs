namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using ActionBase;
    using Checks.Table;
    using Common;
    using Log;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("Table")]
    [RequiredChecks(
        typeof(TableIdentifierReservedWords),
        typeof(TableNameDuplicate),
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    [ReportsChecks(
        typeof(TableLimitsOfColumnsOfDateTime),
        typeof(TableLimitsOfDocumentDateColumns)
    )]
    public class DateTimeOuterLimitsTestAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        protected TableContentRepo contentRepo;

        #endregion

        #region  Constructors

        public DateTimeOuterLimitsTestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, TableContentRepo contentRepo) : base(processLog, testLog, mapping)
        {
            this.contentRepo = contentRepo;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory testFactory)
        {
            using (var uow = testFactory.GetUnitOfWork())
            {
                foreach (var table in uow.GetRepository<Table>().All())
                {
                    ReportAny(TableLimitsOfColumnsOfDateTime.Check(table, contentRepo));
                    ReportAny(TableLimitsOfDocumentDateColumns.Check(table, contentRepo));
                }
            }
        }

        #endregion
    }
}