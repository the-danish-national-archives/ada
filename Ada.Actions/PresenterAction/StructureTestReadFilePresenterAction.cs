namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using ActionBase;
    using Common;
    using IndexEntityLoaders;
    using IngestActions;
    using Log;
    using Repositories;

    #endregion

    [RunsActions(typeof(StructureIngestAction))]
    public class StructureTestReadFilePresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Constructors

        public StructureTestReadFilePresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(AdaUowTarget testSubject)
        {
            using (var structureRepo = new AdaStructureRepo(testSubject.AdaUowFactory, 1000))
            {
                var loader = new StructureEntityLoader(Mapping);
                new StructureIngestAction(GetSubordinateProcessLog(), GetAdaTestLog(), Mapping, structureRepo).Run(
                    new StructureIngestAction.Loader(() => loader.EnumerateFileSystemFolders()));
                structureRepo.Commit();
            }
        }

        #endregion
    }
}