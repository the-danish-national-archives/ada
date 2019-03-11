namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using ActionBase;
    using Checks.Table;
    using Common;
    using Log;
    using Ra.DomainEntities.TableIndex;
    using Repositories;

    #endregion

    [AdaActionPrecondition("tableIndex", "files")]
    [ReportsChecks(
        typeof(TableTestNotInIndex),
        typeof(TableTestMissingTable),
        typeof(TableMissnamedXml),
        typeof(TableTestNumberingGaps) // disabled
    )]
    public class TableIngestPreCheckAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        protected AdaStructureRepo structureRepo;

        #endregion

        #region  Constructors

        public TableIngestPreCheckAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            AdaStructureRepo structureRepo)
            : base(processLog, testLog, mapping)
        {
            this.structureRepo = structureRepo;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            var errorsFound = false;
            var tablesInIndex = new List<string>();
            var tablesOnDisk = new List<string>();

            using (var uow = factory.GetUnitOfWork())
            {
                tablesInIndex = uow.GetRepository<Table>().All().Select(x => x.Folder).ToList();
            }


            tablesOnDisk = structureRepo.GetTableList().ToList();


            foreach (var tableName in tablesOnDisk.Except(tablesInIndex))
            {
                Report(new TableTestNotInIndex(tableName));
                errorsFound = true;
            }

            foreach (var tableName in tablesInIndex.Except(tablesOnDisk))
            {
                Report(new TableTestMissingTable(tableName));
                errorsFound = true;
            }

            using (var repo = new AdaStructureRepo(factory, 0))
            {
                errorsFound |= ReportAny(TableMissnamedXml.Check(repo));
            }


            //            errorsFound |= ReportAny(TableTestNumberingGaps.Check(tablesOnDisk));

            if (errorsFound)
                throw new AdaSkipAllActionException();
        }

        #endregion
    }
}