namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.IO;
    using ActionBase;
    using Checks;
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
        typeof(DiskSpaceWarning), // entrytypeid = '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(SchemaMissingFolder), // '4.F_1'
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema) // '4.F_8'
    )]
    [ReportsChecks(
        typeof(AdaAvSchemaVersionFileIndex),
        typeof(AdaAvSchemaVersionXmlSchema),
        typeof(FileIndexDuplet)
    )]
    [RunsActions(typeof(FileIndexIngestAction))]
    [UILabels("fileIndex")]
    public class ReadFileIndexTestPresenterAction : AdaActionBase<AdaUowTarget>
    {
        #region  Constructors

        public ReadFileIndexTestPresenterAction(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(AdaUowTarget target)
        {
            new SchemaVersionTestAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(),
                target.AdaUowFactory,
                Mapping,
                typeof(AdaAvSchemaVersionFileIndex)).Run("fileIndex");

            new SchemaVersionTestAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(),
                target.AdaUowFactory,
                Mapping,
                typeof(AdaAvSchemaVersionXmlSchema)).Run("XMLSchema");

            var path = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID
                                               + @".1\Indices\fileIndex.xml";
            var xsdpath = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID
                                                  + @".1\Schemas\Standard\\fileIndex.xsd";
            using (var stream = new BufferedProgressStream(new FileInfo(path)))
            using (var xsdStream = new BufferedProgressStream(new FileInfo(xsdpath)))
            {
                using (var indexRepo = new AdaFileIndexRepo(target.AdaUowFactory, 1000))
                {
                    new FileIndexIngestAction(
                        GetSubordinateProcessLog(),
                        GetAdaTestLog(),
                        Mapping,
                        indexRepo).Run(new XmlCouplet(stream, xsdStream, "fileIndex"));
                }
            }


            new AdaSingleQueryAction(GetSubordinateProcessLog(), testLog, target.AdaUowFactory, Mapping)
                .Run(typeof(FileIndexDuplet));
        }

        #endregion
    }
}