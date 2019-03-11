namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.IO;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.ContextDocIndex;
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
        typeof(IndicesContextDocIndex),
        typeof(SchemaMissingFolder),
        typeof(AdaAvSchemaVersionXmlSchema),
        typeof(DiskSpaceWarning),
        typeof(FolderStructureDuplicateMediaNumber),
        typeof(FileIndexInvalid),
        typeof(AdaAvSchemaVersionFileIndex),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed),
        typeof(ContextDocumentationNotWellFormed),
        typeof(ContextDocumentationInvalid)
    )]
    [ReportsChecks(
        typeof(AdaAvSchemaVersionContextDocumentationIndex)
        //typeof(ContextDocMissingFromIndex),
        //typeof(ContextDocMissingDoc),
        //typeof(ContextDocMaxCount)
    )]
    [RunsActions(typeof(ContextDocumentationIngestAction))]
    [RunsActions(typeof(ContextDocumentIntegrityAction))]
    [UILabels("contextDocumentationIndex")]
    public class DocumentationIndexTestPresenterAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Constructors

        public DocumentationIndexTestPresenterAction
        (
            IAdaProcessLog processLog, IAdaTestLog adaTestLog,
            AVMapping localMapping)
            : base(processLog, adaTestLog, localMapping)
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
                typeof(AdaAvSchemaVersionContextDocumentationIndex)).Run("contextDocumentationIndex");

            var path = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID +
                       @".1\Indices\contextDocumentationIndex.XML";
            var xsdpath = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID +
                          @".1\Schemas\Standard\\contextDocumentationIndex.xsd";
            using (var stream = new BufferedProgressStream(new FileInfo(path)))
            using (var xsdStream = new BufferedProgressStream(new FileInfo(xsdpath)))
            {
                var contentRepo = new ContextDocumentationRepo(adaUowFactory);
                new ContextDocumentationIngestAction(
                    GetSubordinateProcessLog(),
                    GetAdaTestLog(),
                    Mapping,
                    contentRepo).Run(
                    new XmlCouplet(
                        stream,
                        xsdStream, "contextDocumentationIndex"));
            }

            new ContextDocumentIntegrityAction(GetSubordinateProcessLog(), GetAdaTestLog(), Mapping).Run(adaUowFactory);

            //new AdaSingleQueryAction(GetSubordinateProcessLog(), GetAdaTestLog(), adaUowFactory, Mapping).Run(
            //    typeof(ContextDocMissingDoc));
            //new AdaSingleQueryAction(GetSubordinateProcessLog(), GetAdaTestLog(), adaUowFactory, Mapping).Run(
            //    typeof(ContextDocMissingFromIndex));
            //new AdaSingleQueryAction(GetSubordinateProcessLog(), GetAdaTestLog(), adaUowFactory, Mapping).Run(
            //    typeof(ContextDocMaxCount));
        }

        #endregion
    }
}