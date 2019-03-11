namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ActionBase;
    using Checks.Documents;
    using Checks.Documents.ContextDocOnDisk;
    using Checks.Documents.DocumentsOnDisk;
    using Checks.Documents.FolderStructure;
    using Checks.Table;
    using ChecksBase;
    using Common;
    using Log;
    using Repositories;
    using TestActions;

    #endregion

    [RequiredChecks(
        typeof(FolderStructureDuplicateMediaNumber) // '4.B.1_2' 
    )]
    [ReportsChecks(
        typeof(IndicesFileIndex),
        typeof(IndicesArchiveIndex),
        typeof(IndicesContextDocIndex),
        typeof(IndicesTableIndex),
        typeof(IndicesDocIndex),
        typeof(FolderStructureFirstMediaMissing),
        typeof(FolderStructureMediaNumberGaps),
        typeof(FolderStructureMissingIndicesFirstMedia),
        typeof(FolderStructureSchemasMissing),
        typeof(FolderStructureContextDocsMissing),
        typeof(FolderStructureTablesMissing),
        typeof(FolderStructureIndicesWrongMedia),
        typeof(FolderStructureSchemasWrongMedia),
        typeof(FolderStructureContextDocsWrongMedia),
        typeof(FolderStructureMultipleDocuments),
        typeof(FolderStructureMultipleTables),
        typeof(ContextDocInvalidObject),
        typeof(ContextDocEmptyFolders),
        typeof(ContextDocEmptyDocFolder),
        typeof(ContextDocNoFolders),
        typeof(ContextDocInvalidFirstNumber),
        typeof(ContextDocBadFileTypes),
        typeof(ContextDocMixedFileTypes),
        typeof(ContextDocDuplicateFolderName),
        typeof(SchemaMissingFolder),
        typeof(SchemaFoldersUnwantedContents),
        typeof(DocumentsInvalidObjects),
        typeof(DocumentsEmptyFolders),
        typeof(DocumentsNoFolders),
        typeof(DocumentsInvalidFirstNumber),
        typeof(DocumentsBadFileTypes),
        typeof(DocumentsMixedFileTypes),
        typeof(DocumentsDuplicateFolderName),
        typeof(ContextDocFileGap),
        typeof(DocumentsFileGap),
        typeof(TableEmptyFolders)
    )]
    [RunsActions(typeof(AvailableDiskSpaceTestAction))]
    public class StructureTestFileCheckPresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Fields

        private readonly string _dbCreationFolder;

        #endregion

        #region  Constructors

        public StructureTestFileCheckPresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, string dbCreationFolder)
            : base(processLog, testLog, mapping)
        {
            _dbCreationFolder = dbCreationFolder;
        }

        #endregion

        #region

        protected override void OnRun(AdaUowTarget target)
        {
            new AvailableDiskSpaceTestAction(GetSubordinateProcessLog(), testLog, target.AdaUowFactory, Mapping).Run(
                new AvailableDiskSpaceTestAction.DBRootDrive(Path.GetPathRoot(_dbCreationFolder)));

            var checks =
                typeof(StructureTestFileCheckPresenterAction)
                    .GetCustomAttributes<ReportsChecksAttribute>(false)
                    .SelectMany(rc => rc.Checks).Where(t => t.IsSubclassOf(typeof(AdaAvDynamicFromSql)));


            var singleQueryAction = new AdaSingleQueryAction(GetSubordinateProcessLog(), testLog, target.AdaUowFactory, Mapping);

            foreach (var check in checks) singleQueryAction.Run(check);

            using (var repo = new AdaStructureRepo(target.AdaUowFactory, 0))
            {
                ReportAny(ContextDocFileGap.Check(repo));
                ReportAny(DocumentsFileGap.Check(repo));
            }
        }

        #endregion
    }
}