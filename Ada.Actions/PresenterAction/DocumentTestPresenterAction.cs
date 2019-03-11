namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.ContextDocIndex;
    using Checks.DocIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Common;
    using Log;
    using Ra.Common.Reflection;
    using Repositories;
    using TestActions;

    #endregion

//    using Ra.DocumentInvestigator.AdaAvChecking.Image.Tiff;

    [RequiredChecks(
        typeof(DiskSpaceWarning), //entrytypeid = '0.3' " // 
        typeof(FolderStructureFirstMediaMissing), //                          + "OR entrytypeid = '4.B.1_1' " // 
        typeof(FolderStructureDuplicateMediaNumber), //                          + "OR entrytypeid = '4.B.1_2' " // 
        typeof(FolderStructureMediaNumberGaps), //                          + "OR entrytypeid = '4.B.1_3' " // 
        typeof(FolderStructureMissingIndicesFirstMedia), //                          + "OR entrytypeid = '4.B.2_1' " // 
        typeof(FolderStructureSchemasMissing), //                          + "OR entrytypeid = '4.B.2_2' " // 
        typeof(FolderStructureTablesMissing), //                          + "OR entrytypeid = '4.B.2_4' " // 
        typeof(IndicesFileIndex), //                          + "OR entrytypeid = '4.C_1' " // 
        typeof(IndicesDocIndex), //                          + "OR entrytypeid = '4.C_5' " // 
        typeof(FileIndexNotWellFormed), //                          + "OR entrytypeid = '4.C.1_1' " // 
        typeof(FileIndexInvalid), //                          + "OR entrytypeid = '4.C.1_2' " // 
        typeof(SchemaMissingFolder), //                          + "OR entrytypeid = '4.F_1' " // 
        typeof(AdaAvSchemaVersionFileIndex), //                          + "OR entrytypeid = '4.F_6' " // 
        typeof(AdaAvSchemaVersionDocIndex), //                          + "OR entrytypeid = '4.F_7' " // 
        typeof(AdaAvSchemaVersionXmlSchema),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(IndicesArchiveIndex),
        typeof(SchemaMissingFolder),
        typeof(ContextDocumentationInvalid),
        typeof(ContextDocMissingDoc)
    )]
    [RunsActions(
        "DocumentsOtherIngestAction"
        //        typeof(DocumentsOtherIngestAction)
    )]
    [RunsActions(
//        typeof(DocumentsOtherIngestAction),
        typeof(GmlTestAction))]
    [UILabels("Dokumenter", "", "", "")]
    public class DocumentTestPresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Fields

        private readonly string _dbCreationFolder;
        private readonly int? _highPageCount;
        private readonly int _tiffChecks;

        #endregion

        #region  Constructors

        public DocumentTestPresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, int tiffChecks, int? highPageCount, string dbCreationFolder)
            : base(processLog, testLog, mapping)
        {
            _tiffChecks = tiffChecks;
            _highPageCount = highPageCount;
            _dbCreationFolder = dbCreationFolder;
        }

        #endregion

        #region

        protected override void OnRun(AdaUowTarget testSubject)
        {
            using (var fullDocumentRepoHacker =
                new FullDocumentRepoHacker(
                    testSubject.AdaUowFactory,
                    Mapping.AVID,
                    _dbCreationFolder,
                    false))
            {
                var documentsOtherIngestAction = AdaActionContainer.Instance.GetAction("DocumentsOtherIngestAction")
                        ?.GetConstructor(new[] {typeof(IAdaProcessLog), typeof(IAdaTestLog), typeof(AVMapping), typeof(int), typeof(int?)})
                        ?.Invoke(new object[] {GetSubordinateProcessLog(), GetAdaTestLog(), Mapping, _tiffChecks, _highPageCount})
                    as AdaActionBase<FullDocumentRepoHacker>;

                if (documentsOtherIngestAction != null)
                    documentsOtherIngestAction.ProgressCallback = ProgressCallback;

                documentsOtherIngestAction?.Run(fullDocumentRepoHacker);
            }

            using (var repo = new DocumentIndexRepo(testSubject.AdaUowFactory, 1000))
            {
                var gmlTestAction = new GmlTestAction(GetSubordinateProcessLog(), GetAdaTestLog(), testSubject.AdaUowFactory, Mapping);

                gmlTestAction.ProgressCallback = ProgressCallback;

                gmlTestAction
                    .Run(repo);
            }
        }

        #endregion
    }
}