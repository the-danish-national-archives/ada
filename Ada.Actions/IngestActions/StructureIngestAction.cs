namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ActionBase;
    using Checks.Documents.FolderStructure;
    using Common;
    using IndexEntityLoaders;
    using Log;
    using Ra.DomainEntities.FileSystem;
    using Repositories;

    #endregion

    [AdaActionPrecondition]
    [ReportsChecks(typeof(FolderStructureDuplicateMediaNumber))]
    public class StructureIngestAction : AdaIngestAction<FileSystemFolder, AdaStructureRepo, StructureIngestAction.Loader>
    {
        #region  Constructors

        public StructureIngestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, AdaStructureRepo structureRepo)
            : base(processLog, testLog, mapping, structureRepo)
        {
        }

        #endregion

        #region

        protected override void OnRun(Loader loader)
        {
            try
            {
                foreach (var folder in loader.GetFolders()) TargetRepository.SaveEntity(folder);
                TargetRepository.Commit();
            }
            catch (DuplicateMediaNumbersException ed)
            {
                Report(new FolderStructureDuplicateMediaNumber(ed.Name, ed.Count));
            }
        }

        #endregion

        #region Nested type: Loader

        public class Loader
        {
            #region  Fields

            private readonly Func<IEnumerable<FileSystemFolder>> _callee;

            #endregion

            #region  Constructors

            public Loader(Func<IEnumerable<FileSystemFolder>> callee)
            {
                _callee = callee;
            }

            #endregion

            #region

            public IEnumerable<FileSystemFolder> GetFolders()
            {
                return _callee();
            }

            public override string ToString()
            {
                return "Folders";
            }

            #endregion
        }

        #endregion
    }
}