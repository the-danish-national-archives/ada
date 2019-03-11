namespace Ada.Actions.IngestActions
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using ActionBase;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Common;
    using Log;
    using Repositories;

    #endregion

    [AdaActionPrecondition("files")]
    [RequiredChecks(
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // = '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // = '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // = '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // = '4.B.2_2'
        typeof(FolderStructureTablesMissing), // = '4.B.2_4'
        typeof(IndicesFileIndex), // = '4.C_1'
        typeof(SchemaMissingFolder) // = '4.F_1'
    )]
    public class Md5IngestAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public Md5IngestAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory factory)
        {
            using (var repo = new AdaStructureRepo(
                factory,
                0))
            {
                var totalDoc = repo.TotalFiles();
                var docI = 0;

                void Reporter()
                {
                    ++docI;
                    ProgressCallback?.Invoke($"{docI} ud af {totalDoc}");
                }

                try
                {
                    var md5 = MD5.Create();
                    foreach (var file in repo.EnumerateFiles())
                    {
                        var path = Path.Combine(
                            Mapping.GetMediaRoot(file.MediaNumber),
                            file.RelativePath,
                            file.Name);
                        if (File.Exists(path))
                            using (var stream = File.OpenRead(path))
                            {
                                var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                                file.CheckSum = hash;
                                repo.UpdateMD5(file);
                            }

                        Reporter();
                    }
                }
                finally
                {
                    repo.Commit();
                }
            }
        }

        #endregion
    }
}