namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.Linq;
    using System.Reflection;
    using ActionBase;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Common;
    using Log;

    #endregion

    [RequiredChecks(
        typeof(FolderStructureFirstMediaMissing), //  '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), //  = '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), //  = '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), //  = '4.B.2_1'
        typeof(FolderStructureSchemasMissing), //  = '4.B.2_2'
        typeof(FolderStructureTablesMissing), //  = '4.B.2_4'
        typeof(IndicesFileIndex), //  = '4.C_1'
        typeof(FileIndexNotWellFormed), //  = '4.C.1_1'
        typeof(FileIndexInvalid), //  = '4.C.1_2'
        typeof(SchemaMissingFolder), //  = '4.F_1'
        typeof(AdaAvSchemaVersionFileIndex), //  = '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema) //  = '4.F_8'
    )]
    [ReportsChecks(
        typeof(FileIndexFilesMissing),
        typeof(FileIndexFilesNotInIndex),
        typeof(FileIndexBadCheckSum))]
    public class StructureTestMd5CheckPresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Constructors

        public StructureTestMd5CheckPresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(AdaUowTarget target)
        {
            var checks =
                typeof(StructureTestMd5CheckPresenterAction).GetCustomAttributes<ReportsChecksAttribute>(false).SelectMany(rc => rc.Checks);


            var singleQueryAction = new AdaSingleQueryAction(GetSubordinateProcessLog(), testLog, target.AdaUowFactory, Mapping);

            foreach (var check in checks) singleQueryAction.Run(check);
        }

        #endregion
    }
}