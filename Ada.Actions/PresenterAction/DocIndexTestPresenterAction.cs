namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.IO;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.DocIndex;
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
        typeof(AdaAvSchemaVersionFileIndex),
        typeof(FolderStructureFirstMediaMissing),
        typeof(FolderStructureMediaNumberGaps),
        typeof(FolderStructureMissingIndicesFirstMedia),
        typeof(FolderStructureSchemasMissing),
        typeof(FolderStructureTablesMissing),
        typeof(IndicesFileIndex),
        typeof(SchemaMissingFolder),
        typeof(DiskSpaceWarning),
        typeof(FolderStructureDuplicateMediaNumber),
        typeof(FileIndexNotWellFormed),
        typeof(FileIndexInvalid),
        typeof(AdaAvSchemaVersionXmlSchema),
        typeof(IndicesDocIndex),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(DocIndexNotWellFormed),
        typeof(DocIndexInvalid)
    )]
    [ReportsChecks(
        typeof(AdaAvSchemaVersionDocIndex)
    )]
    [RunsActions(typeof(DocumentIndexIngestAction), typeof(DocIndexIntegrityCheckAction))]
    [UILabels("docIndex")]
    public class DocIndexTestPresenterAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public DocIndexTestPresenterAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog adaTestLog,
            AVMapping mapping)
            : base(processLog, adaTestLog, mapping)
        {
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory adaUowFactory)
        {
            new SchemaVersionTestAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(),
                adaUowFactory,
                Mapping,
                typeof(AdaAvSchemaVersionDocIndex)).Run("docIndex");

            var path = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID
                                               + @".1\Indices\docIndex.xml";
            var xsdpath = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID
                                                  + @".1\Schemas\Standard\\docIndex.xsd";

            var xmlStream = new BufferedProgressStream(new FileInfo(path));
            using (var xsdStream = new BufferedProgressStream(new FileInfo(xsdpath)))
            using (var indexRepo = new DocumentIndexRepo(adaUowFactory, 1000))
            {
                new DocumentIndexIngestAction(
                    GetSubordinateProcessLog(),
                    GetAdaTestLog(), Mapping, indexRepo).Run(
                    new XmlCouplet(xmlStream, xsdStream, "docIndex"));
            }


            new DocIndexIntegrityCheckAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(), Mapping).Run(adaUowFactory);
        }

        #endregion
    }
}