namespace Ada.Actions.PresenterAction
{
    #region Namespace Using

    using System.IO;
    using ActionBase;
    using Checks;
    using Checks.ArchiveIndex;
    using Checks.Documents;
    using Checks.Documents.FolderStructure;
    using Checks.FileIndex;
    using Checks.TableIndex;
    using Common;
    using IngestActions;
    using Log;
    using Ra.Common;
    using Ra.Common.Reflection;
    using Repositories;
    using TestActions;

    #endregion

    [RequiredChecks(
        typeof(DiskSpaceWarning), // '0.3'
        typeof(FolderStructureFirstMediaMissing), // '4.B.1_1'
        typeof(FolderStructureDuplicateMediaNumber), // '4.B.1_2'
        typeof(FolderStructureMediaNumberGaps), // '4.B.1_3'
        typeof(FolderStructureMissingIndicesFirstMedia), // '4.B.2_1'
        typeof(FolderStructureSchemasMissing), // '4.B.2_2'
        typeof(FolderStructureTablesMissing), // '4.B.2_4'
        typeof(IndicesFileIndex), // '4.C_1'
        typeof(IndicesTableIndex), // '4.C_4'
        typeof(FileIndexNotWellFormed), // '4.C.1_1'
        typeof(FileIndexInvalid), // '4.C.1_2'
        typeof(SchemaMissingFolder), // '4.F_1'
        typeof(AdaAvSchemaVersionFileIndex), // '4.F_6'
        typeof(AdaAvSchemaVersionXmlSchema),
        typeof(AdaAvSchemaVersionArchieIndex),
        typeof(ArchiveIndexInvalid),
        typeof(ArchiveIndexNotWellFormed)
    )]
    [ReportsChecks(
        typeof(AdaAvSchemaVersionTableIndex),
        typeof(TableIndexInvalidDescription),
        typeof(TableIndexInvalidColumnDescription)
    )]
    [RunsActions(typeof(TableIndexIngestAction))]
    [UILabels("tableIndex")]
    public class TableIndexIngestPresenterAction : AdaActionBase<IAdaUowFactory>
    {
        #region  Fields

        private readonly IAdaUowFactory _modelFactory;
        private readonly IAdaUowFactory _uowFactory;

        #endregion

        #region  Constructors

        public TableIndexIngestPresenterAction
        (
            IAdaProcessLog processLog,
            IAdaTestLog testLog,
            AVMapping mapping,
            IAdaUowFactory uowFactory,
            AdaAvUowFactory modelFactory)
            : base(processLog, testLog, mapping)
        {
            _uowFactory = uowFactory;
            _modelFactory = modelFactory;
        }

        #endregion

        #region

        protected override void OnRun(IAdaUowFactory target)
        {
            new SchemaVersionTestAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(),
                target,
                Mapping,
                typeof(AdaAvSchemaVersionTableIndex)).Run("tableIndex");

            var path = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID
                                               + @".1\Indices\tableIndex.XML";
            var xsdpath = Mapping.GetMediaRoot(1) + Mapping.AVID.FullID
                                                  + @".1\Schemas\Standard\\tableIndex.xsd";

            using (var stream = new BufferedProgressStream(new FileInfo(path)))
            using (var xsdStream = new BufferedProgressStream(new FileInfo(xsdpath)))
            {
                if (_modelFactory.DataBaseExists())
                {
                    _modelFactory.DeleteDataBase();
                    _modelFactory.CreateDataBase();
                }


                using (var contentRepo = new TableContentRepo(_uowFactory, 1000))
                using (var modelRepo = new TableContentRepo(_modelFactory, 1000))
                {
                    new TableIndexIngestAction(
                        GetSubordinateProcessLog(),
                        GetAdaTestLog(),
                        Mapping,
                        new TableIndexRepo(target),
                        modelRepo,
                        contentRepo).Run(new XmlCouplet(stream, xsdStream, "tableIndex"));
                }
            }

            new AdaSingleQueryAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(),
                target,
                Mapping).Run(typeof(TableIndexInvalidDescription));
            new AdaSingleQueryAction(
                GetSubordinateProcessLog(),
                GetAdaTestLog(),
                target,
                Mapping).Run(typeof(TableIndexInvalidColumnDescription));
        }

        #endregion
    }
}