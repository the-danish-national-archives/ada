namespace Ada.Actions.TestActions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using ActionBase;
    using Checks.DocIndex;
    using Checks.Gml;
    using ChecksBase;
    using Common;
    using IngestActions;
    using log4net;
    using Log;
    using Ra.Common;
    using Ra.Common.Xml;
    using Ra.Common.Xml.GeoData;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    [AdaActionPrecondition("documentIndex", "files")]
    [ReportsChecks(typeof(GmlOgcSchemasMissing), typeof(GmlOgcSchemasInvalid), typeof(GmlSchemaInvalid),
        typeof(GmlSchemaNotWellFormed), typeof(GmlSchemaIllegalEncoding), typeof(GmlNotWellFormed),
        typeof(GmlInvalid), typeof(GmlIllegalEncoding), typeof(GmlMissingProlog),
        typeof(GmlSchemaOgcImportError), typeof(GmlSchemaGeometryMissingError), typeof(GmlSchemaGeometrySubstitutionGroupError),
        typeof(GmlSchemaMissingAnnotation), typeof(GmlSchemaExtensionBaseError), typeof(GmlSchemaMissingFeature),
        typeof(GmlSchemaMissingFeatureAnnotation), typeof(GmlSchemaMissingOgcRef), typeof(GmlSchemaLocationError),
        typeof(GmlRootElementError), typeof(GmlNameSpaceDeclarationerror), typeof(GmlIllegalEPSG),
        typeof(GmlIllegalDimension), typeof(GmlIllegalBounds), typeof(GmlFeatureMemberNotFound),
        typeof(GmlNoGeometryError), typeof(GmlGeometryOutOfBounds), typeof(DocIndexInvalidGmlXsd),
        typeof(GmlSchemaNotFound), typeof(GmlFileTooLarge))]
    [RunsActions(typeof(GeoDataIngestAction))]
    public class GmlTestAction : AdaActionBase<DocumentIndexRepo>
    {
        #region  Fields

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private readonly IAdaUowFactory testFactory;

        #endregion

        #region  Constructors

        public GmlTestAction(IAdaProcessLog processLog, IAdaTestLog testLog, IAdaUowFactory testFactory, AVMapping mapping)
            : base(processLog, testLog, mapping)
        {
            this.testFactory = testFactory;
        }

        #endregion

        #region

        protected override void OnRun(DocumentIndexRepo documentIndexRepo)
        {
            var anXsdFileNeed = false;

            var gmlFiles = documentIndexRepo.EnumerateDocs(FileTypeEnum.Gml);


            var totalDoc = documentIndexRepo.TotalDocumentCount();
            var docI = totalDoc - documentIndexRepo.TotalDocumentCount(FileTypeEnum.Gml);

            void Reporter()
            {
                ++docI;
                ProgressCallback?.Invoke($"{docI} ud af {totalDoc}");
            }


            foreach (var file in gmlFiles)
                if (file.GmlXsd != string.Empty)
                    anXsdFileNeed = true;


            if (!anXsdFileNeed)
            {
                foreach (var file in documentIndexRepo.EnumerateDocs(FileTypeEnum.Gml))
                {
                    var folderPath = Path.Combine(
                        Mapping.GetMediaPath(int.Parse(file.MediaId)),
                        "Documents",
                        file.DocumentFolder,
                        file.DocumentId);

                    var gmlPath = Path.Combine(folderPath, "1.gml");
                    Report(new DocIndexInvalidGmlXsd(Mapping.GetRelativePath(new FileInfo(gmlPath))));
                }

                return;
            }

            var schemaMissing =
                ReportAny(GmlOgcSchemasMissing.Check(testFactory));


            ReportAny(GmlOgcSchemasInvalid.Check(testFactory));

            if (schemaMissing)
                return;


            var localSharedPath = Path.Combine(
                Mapping.GetMediaRoot(1),
                Mapping.AVID.FullID + ".1",
                "schemas",
                "localShared");
            var standardPath = Path.Combine(
                Mapping.GetMediaRoot(1),
                Mapping.AVID.FullID + ".1",
                "schemas",
                "standard");


            var errorMap =
                new Dictionary<XmlEventType, Type>
                {
                    {XmlEventType.Exception, typeof(AdaAvXmlViolation)},
                    {XmlEventType.SchemaValidationError, typeof(GmlSchemaInvalid)},
                    {XmlEventType.SchemaWellFormedNessError, typeof(GmlSchemaNotWellFormed)},
                    {XmlEventType.SchemaDeclaredEncodingIllegal, typeof(GmlSchemaIllegalEncoding)},
                    {XmlEventType.XmlWellFormednessError, typeof(GmlNotWellFormed)},
                    {XmlEventType.XmlValidationError, typeof(GmlInvalid)},
                    {XmlEventType.XmlValidationWarning, typeof(GmlInvalid)},
                    {XmlEventType.XmlDeclaredEncodingIllegal, typeof(GmlIllegalEncoding)},
                    {XmlEventType.XmlMissingProlog, typeof(GmlMissingProlog)}
                };

            IXmlEventLogger logger = new XmlEventLogger(errorMap, Mapping);
            logger.CallBack = Report;


            var schemas = new List<string>();

            using (var uowTest = testFactory.GetUnitOfWork())
            {
                var repository = uowTest.GetRepository<AdaGisSchema>();

                var adaGisSchema = repository.All();

                foreach (var gisSchema in adaGisSchema) schemas.Add(gisSchema.FileName);
            }

            var filter = new XmlEventFilter();
            var streams = new List<BufferedProgressStream>();

            foreach (var schema in schemas)
            {
                var path = Path.Combine(localSharedPath, schema);
                streams.Add(new BufferedProgressStream(new FileInfo(path)));
            }

            var xmlxsdpath = Path.Combine(standardPath, "XMLSchema.xsd");
            streams.Add(new BufferedProgressStream(new FileInfo(xmlxsdpath)));


            var gmlErrorMap =
                new Dictionary<GeoDataEventType, Type>
                {
                    {GeoDataEventType.SchemaOgcImportError, typeof(GmlSchemaOgcImportError)}, // "5.G_8";
                    {GeoDataEventType.SchemaGeometryMissingError, typeof(GmlSchemaGeometryMissingError)},
                    // "5.G_9";
                    {
                        GeoDataEventType.SchemaGeometrySubstitutionGroupError,
                        typeof(GmlSchemaGeometrySubstitutionGroupError)
                    }, // "5.G_10";
                    {GeoDataEventType.SchemaMissingAnnotation, typeof(GmlSchemaMissingAnnotation)}, // "5.G_11";
                    {GeoDataEventType.SchemaExtensionBaseError, typeof(GmlSchemaExtensionBaseError)}, // "5.G_12";
                    {GeoDataEventType.SchemaMissingFeature, typeof(GmlSchemaMissingFeature)}, // "5.G_13";
                    {GeoDataEventType.SchemaMissingFeatureAnnotation, typeof(GmlSchemaMissingFeatureAnnotation)},
                    // "5.G_14";
                    {GeoDataEventType.SchemaMissingOgcRef, typeof(GmlSchemaMissingOgcRef)}, // "5.G_17";
                    {GeoDataEventType.GmlSchemaLocationError, typeof(GmlSchemaLocationError)}, // "5.G_19";
                    {GeoDataEventType.GmlRootElementError, typeof(GmlRootElementError)}, // "5.G_22";
                    {GeoDataEventType.GmlNameSpaceDeclarationerror, typeof(GmlNameSpaceDeclarationerror)},
                    // "5.G_23";
                    {GeoDataEventType.GmlIllegalEPSG, typeof(GmlIllegalEPSG)}, // "5.G_24";
                    {GeoDataEventType.GmlIllegalDimension, typeof(GmlIllegalDimension)}, // "5.G_25";
                    {GeoDataEventType.GmlIllegalBounds, typeof(GmlIllegalBounds)}, // "5.G_26";
                    {GeoDataEventType.GmlFeatureMemberNotFound, typeof(GmlFeatureMemberNotFound)}, // "5.G_27";
                    {GeoDataEventType.GmlNoGeometryError, typeof(GmlNoGeometryError)}, // "5.G_28";
                    {GeoDataEventType.GmlGeometryOutOfBounds, typeof(GmlGeometryOutOfBounds)} // "5.G_29";
                };

            var schemalogger = new GeoDataEventLogger(Report, gmlErrorMap, Mapping);


            var rxValidXsdName = new Regex(@"^localschema\d+.xsd$|\d+.xsd$");


            var reader = new ArchivalGeoDataReader(filter, streams);


//            GeoDataEventLogger schemalogger = new GeoDataEventLogger(this.ReportLogEntry);
            reader.GeoDataEvent += schemalogger.GeoDataEventHandler;


            foreach (var file in documentIndexRepo.EnumerateDocs(FileTypeEnum.Gml))
            {
                Reporter();

                var folderPath = Path.Combine(
                    Mapping.GetMediaPath(int.Parse(file.MediaId)),
                    "Documents",
                    file.DocumentFolder,
                    file.DocumentId);

                var gmlPath = Path.Combine(folderPath, "1.gml");
                var xsdPath = Path.Combine(file.GmlXsd == "1.xsd" ? folderPath : localSharedPath, file.GmlXsd);

                var xsdFileInfo = new FileInfo(xsdPath);
                var gmlFileInfo = new FileInfo(gmlPath);

                var match = rxValidXsdName.Match(file.GmlXsd.ToLower());

                if (!match.Success)
                {
                    Report(new DocIndexInvalidGmlXsd(Mapping.GetRelativePath(gmlFileInfo)));
                    continue;
                }


                if (!xsdFileInfo.Exists)
                {
                    Report(new GmlSchemaNotFound(Mapping.GetRelativePath(xsdFileInfo)));
                    continue;
                }


                if (!gmlFileInfo.Exists) continue; // caught in structure test

                if (gmlFileInfo.Length >= 1000000000)
                {
                    Report(new GmlFileTooLarge(Mapping.GetRelativePath(gmlFileInfo)));
                    continue;
                }

                var gmlStream = new BufferedProgressStream(gmlFileInfo);
                var xsdStream = new BufferedProgressStream(xsdFileInfo);
                new GeoDataIngestAction(GetSubordinateProcessLog(), testLog, reader, logger, Mapping).Run
                    (new XmlCouplet(gmlStream, xsdStream, gmlFileInfo.FullName));
            }
        }

        #endregion
    }
}