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
        typeof(AdaAvSchemaVersionXmlSchema), //                          + "OR entrytypeid = '4.F_8' " // 

        // Added due to ADA-29 to correctly report green lamps
        typeof(IndicesArchiveIndex),
        typeof(ContextDocumentationInvalid),
        typeof(ContextDocMissingDoc),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed)
    )]
    [RunsActions(
        "DocumentsOtherIngestAction"
        //        typeof(DocumentsOtherIngestAction)
    )]
    [UILabels("KontekstDokumentation")]
    public class ContextDocumentTestPresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Fields

        private readonly string _dbCreationFolder;
        private readonly int _tiffChecks;
        private readonly int? _highPageCount;

        #endregion

        #region  Constructors

        public ContextDocumentTestPresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, int tiffChecks, int? highPageCount, string dbCreationFolder)
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
                    true))
            {
                var documentsOtherIngestAction = AdaActionContainer.Instance.GetAction("DocumentsOtherIngestAction")
                        ?.GetConstructor(new[] {typeof(IAdaProcessLog), typeof(IAdaTestLog), typeof(AVMapping), typeof(int), typeof(int?)})
                        ?.Invoke(new object[] {GetSubordinateProcessLog(), GetAdaTestLog(), Mapping, _tiffChecks, _highPageCount})
                    as AdaActionBase<FullDocumentRepoHacker>;

                if (documentsOtherIngestAction != null)
                    documentsOtherIngestAction.ProgressCallback = ProgressCallback;

                documentsOtherIngestAction?.Run(fullDocumentRepoHacker);
            }
        }

        #endregion
    }
}