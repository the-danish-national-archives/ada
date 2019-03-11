namespace Ada.Common.TestActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Ada.Common.AdaAvChecks.Table;
    using Ada.Entities;
    using Ada.Entities.AdaAvCheckNotifications;

    using global::Ada.Common;
    using global::Ada.Log;
    using global::Ada.Repositories;
    
    using Ra.DomainEntities.TableIndex;

    public class TableIngestPreChcek : AdaActionBase<IAdaUowFactory>
    {
        protected override void OnRun(IAdaUowFactory factory)
        {
            bool errorsFound = false;
            var tablesInIndex = new List<string>();
            var tablesOnDisk = new List<string>();

            using (var uow = factory.GetUnitOfWork())
            {
                tablesInIndex = uow.GetRepository<Table>().All().Select(x => x.Folder).ToList();
            }

            using (var structrepo = new AdaStructureRepo(
                            new AdaTestUowFactory(
                            this.Mapping.AVID, "test",
                            new DirectoryInfo(Properties.Settings.Default.DBCreationFolder)),
                            1000))
            {
                tablesOnDisk = structrepo.GetTableList().ToList();
            }

            foreach (var tableName in tablesOnDisk.Except(tablesInIndex))
            {
                Report(new TableTestNotInIndex(folderName: tableName));
                errorsFound = true;
            }

            foreach (var tableName in tablesInIndex.Except(tablesOnDisk))
            {
                Report(new TableTestMissingTable(tableName: tableName));
                errorsFound = true;
            }

            errorsFound |= ReportAny(TableTestNumberingGaps.Check(tablesOnDisk));

            if (errorsFound)
                throw new AdaSkipAllActionException();
        }

        public TableIngestPreChcek(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

    }
}

