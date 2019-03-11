namespace Ada.Actions.Rewrite
{
    #region Namespace Using

    using System.IO;
    using ActionBase;
    using Checks;
    using Checks.DocIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.Table;
    using Checks.TableIndex;
    using Common;
    using EntityLoaders;
    using Log;
    using Repositories;

    #endregion

    [AdaActionPrecondition("files")]
    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(FileIndexNotWellFormed), // '4.C.1_1'
        typeof(FileIndexInvalid), // '4.C.1_2'
        typeof(TableNameDuplicate), // '4.C.5_3'
        typeof(TableIndexNotWellFormed), // '4.C.1_7'
        typeof(TableIndexInvalid), // '4.C.1_8'
        typeof(SchemaMissingFolder), // '4.F_1'
        typeof(AdaAvSchemaVersionTableIndex), // '4.F_5'
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema), // '4.F_8'
        typeof(DocIndexNotWellFormed), // '6.C_2'
        typeof(TableIndexDuplicateColumnId), // '6.C_3'
        typeof(TableIndexReferencedTableMissing), // '6.C_19'
        typeof(TableIndexMissingParentColumns), // '6.C_20'
        typeof(TableIndexForeignKeyColumnMissingInParent), // '6.C_22'
        typeof(TableIdentifierReservedWords), // '6.C_28'
        typeof(TableFolderSequenceInvalidStart),
        typeof(TablesFolderSequenceGaps)
    )]
    public class FileIndexRewriteAction : AdaActionBase<string>
    {
        #region  Fields

        // TODO remove hack for clear of rewrite
        private readonly IAdaProcessLog processLog;
        protected IAdaUowFactory TestUowFactory;

        #endregion

        #region  Constructors

        public FileIndexRewriteAction
            (IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping, IAdaUowFactory testFactory)
            : base(processLog, testLog, mapping)
        {
            TestUowFactory = testFactory;
            this.processLog = processLog;
        }

        #endregion

        #region

        protected override void OnRun(string path)
        {
            if (File.Exists(path + ".original")) File.Delete(path + ".original");


            RenameFile(path, path + ".original");

            using (var fs = new FileStream(path, FileMode.Create))
            {
                var writer = new FileIndexWriter();
                writer.Open(fs);

                var repo = new AdaFileIndexRepo(TestUowFactory, 1000);
                foreach (var file in repo.EnumerateFiles()) writer.WriteElement(file, Mapping.AVID);
                repo.Dispose();
                writer.Close();
            }

            File.Delete(path + ".original");
        }

        private void RenameFile(string oldFilename, string newFilename) //Proper rename instead?
        {
            var file = new FileInfo(oldFilename);

            if (file.Exists) file.MoveTo(newFilename);
        }

        public ActionRunResult ResetAndRun(string path)
        {
            var previousProcessLogEntry = processLog.GetExistingProcessLog(GetId(path));

            if (previousProcessLogEntry != null)
                Clear(previousProcessLogEntry);

            return Run(path);
        }

        #endregion
    }
}