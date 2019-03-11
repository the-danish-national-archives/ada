namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.IO;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Common;
    using IngestActions;
    using Log;
    using Ra.Common;
    using Ra.Common.Reflection;
    using Repositories;
    using TestActions;

    #endregion

    [RequiredChecks(
        typeof(FolderStructureFirstMediaMissing),
        typeof(FolderStructureMediaNumberGaps),
        typeof(FolderStructureMissingIndicesFirstMedia),
        typeof(FolderStructureSchemasMissing),
        typeof(FolderStructureTablesMissing),
        typeof(IndicesFileIndex),
        typeof(IndicesArchiveIndex),
        typeof(SchemaMissingFolder))]
    [ReportsChecks(
        typeof(AdaAvSchemaVersionArchieIndex))]
    [RunsActions(typeof(ArchiveIndexIngestAction))]
    [UILabels("archiveIndex")]
    public class ArchiveIndexTestPresenterAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public ArchiveIndexTestPresenterAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog adaTestLog,
            AVMapping mapping) : base(processLog, adaTestLog, mapping)
        {
        }

        #endregion

        #region

        protected override IEnumerable<AdaActionRequirement> GetOtherRequirements()
        {
            return new[]
            {
                AdaActionRequirement.FromChecks(
                    new AvailableDiskSpaceTestAction.DBRootDrive(),
                    typeof(DiskSpaceWarning)),
                AdaActionRequirement.FromChecks(
                    new StructureIngestAction.Loader(() => null),
                    typeof(FolderStructureDuplicateMediaNumber)),
                new AdaActionRequirement(
                    typeof(FileIndexIngestAction),
                    new XmlCouplet("fileIndex"),
                    typeof(FileIndexNotWellFormed),
                    typeof(FileIndexInvalid)),
                new AdaActionRequirement(
                    typeof(SchemaVersionTestAction),
                    "fileIndex",
                    typeof(AdaAvSchemaVersionFileIndex)),
                new AdaActionRequirement(
                    typeof(SchemaVersionTestAction),
                    "XMLSchema",
                    typeof(AdaAvSchemaVersionXmlSchema))
            };
        }

        protected override void OnRun(IAdaUowFactory adaUowFactory)
        {
            try
            {
                new SchemaVersionTestAction(
                        GetSubordinateProcessLog(),
                        GetAdaTestLog(),
                        adaUowFactory,
                        Mapping,
                        typeof(AdaAvSchemaVersionArchieIndex))
                    .Run("archiveIndex");

                var path = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID +
                           @".1\Indices\archiveIndex.XML";
                var xsdpath = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID +
                              @".1\Schemas\Standard\\archiveIndex.xsd";
                using (var stream = new BufferedProgressStream(new FileInfo(path)))
                using (var xsdStream = new BufferedProgressStream(new FileInfo(xsdpath)))
                {
                    var indexRepo = new ArchiveIndexRepo(adaUowFactory);

                    new ArchiveIndexIngestAction(
                            GetSubordinateProcessLog(),
                            GetAdaTestLog(),
                            Mapping,
                            indexRepo)
                        .Run(new XmlCouplet(stream, xsdStream, "archiveIndex"));
                }
            }
            catch (AdaSkipAllActionException)
            {
                // Stop action
            }
        }

        #endregion
    }
}