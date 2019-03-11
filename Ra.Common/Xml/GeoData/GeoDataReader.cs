// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchivalGeoDataReader.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The geo data reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Ra.Common.Xml.GeoData
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    #endregion

    public delegate void GeoDataEventHandler(object sender, GeoDataEventArgs e);

    /// <summary>
    ///     The geo data reader.
    /// </summary>
    public class ArchivalGeoDataReader : ArchivalXmlReader, IArchivalGeoDataReader
    {
        #region  Fields

        private readonly XmlSchemaSet baseSchemaSet;

        private readonly IEnumerable<BufferedProgressStream> baseSchemaStreams;

        private readonly XNamespace OpenGisNs;

        private Bounds currentBounds;

        private GisDimensions? currentDimensions;

        private readonly GeoDataReaderSettings geoDataSettings;

        private XmlSchema localschema;

        #endregion

        #region  Constructors

        public ArchivalGeoDataReader(IXmlEventFilter eventFilter, IEnumerable<BufferedProgressStream> baseSchemaStreams, ArchivalXmlReaderSettings xmlSettings = null, GeoDataReaderSettings geoDataSettings = null)
            : base(eventFilter, xmlSettings)
        {
            baseSchemaSet = new XmlSchemaSet {XmlResolver = null};
            this.baseSchemaStreams = baseSchemaStreams;
            baseSchemaSet.ValidationEventHandler += SchemaValidationEventHandler;
            this.geoDataSettings = geoDataSettings ?? new GeoDataReaderSettings();
            OpenGisNs = Constants.OpenGisNs;
        }

        #endregion

        #region Properties

        private string ExpectedSchemaLocation
        {
            get
            {
                var schemaLocation = localschema.TargetNamespace;
                if (currentSchemaStream.file.Name.Equals("1.xsd"))
                    schemaLocation += " ./";
                else
                    schemaLocation += " ../../../Schemas/localShared/";
                schemaLocation += currentSchemaStream.file.Name;
                return schemaLocation;
            }
        }

        #endregion

        #region IArchivalGeoDataReader Members

        public event GeoDataEventHandler GeoDataEvent;


        public IEnumerable<XElement> FeatureStream()
        {
            return ElementStream("gml:featureMember");
        }

        #endregion

        #region

        private void CheckBounds(XElement boundedByElement)
        {
            var envelopeElement = boundedByElement.Element(OpenGisNs + Constants.EnvelopeName);

            var dimension = envelopeElement.Attribute(Constants.DimensionName);
            if (dimension == null || !geoDataSettings.AcceptableDimensions.Contains(dimension.Value))
            {
                OnGmlEvent(GeoDataEventType.GmlIllegalDimension, currentXmlStream.file); // 5.G_27
                currentDimensions = null;
            }
            else
            {
                currentDimensions = (GisDimensions) Convert.ToInt16(dimension.Value);
            }

            var datum = envelopeElement.Attribute(Constants.DatumName);
            if (datum == null || !geoDataSettings.AcceptableDatums.Contains(datum.Value)) OnGmlEvent(GeoDataEventType.GmlIllegalEPSG, currentXmlStream.file); // 5.G_26

            var lowerCorner = envelopeElement.Element(OpenGisNs + Constants.BoundsLowerCornerName);
            var upperCorner = envelopeElement.Element(OpenGisNs + Constants.BoundsUpperCornerName);

            if (lowerCorner == null || upperCorner == null)
            {
                OnGmlEvent(GeoDataEventType.GmlIllegalBounds, currentXmlStream.file); // 5.G_28
            }
            else
            {
                var declaredBounds = new Bounds(lowerCorner.Value, upperCorner.Value);
                currentBounds = declaredBounds;
                if (!declaredBounds.IsWithinOrEqualTo(geoDataSettings.Bounds)) OnGmlEvent(GeoDataEventType.GmlIllegalBounds, currentXmlStream.file); // 5.G_28 TODO: Testsæt og eventuelle andre fejlkriterier
            }
        }


        private void CheckRootCollection()
        {
            reader.MoveToContent();
            if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("gml:FeatureCollection"))
            {
                var attributelist = reader.ReadAttributes();
                if (!NameSpaceAttributesOk(attributelist)) OnGmlEvent(GeoDataEventType.GmlNameSpaceDeclarationerror, currentXmlStream.file); // 5.G_25

                if (!LocalSchemaLocationOk(attributelist)) OnGmlEvent(GeoDataEventType.GmlSchemaLocationError, currentXmlStream.file); // 5.G_21
            }
            else
            {
                OnGmlEvent(GeoDataEventType.GmlRootElementError, currentXmlStream.file); // 5.G_24
            }

            reader.Read();
            reader.MoveToContent();
            if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("gml:boundedBy")) CheckBounds(XNode.ReadFrom(reader) as XElement);
        }


        private void CheckSchema(XmlSchema schema, FileInfo sourceFileInfo)
        {
            if (!schema.Includes.OfType<XmlSchemaImport>()
                .ToList()
                .Exists(x => x.Namespace.Equals(Constants.OpenGisNs) && x.SchemaLocation.Equals(Constants.OpenGisSchemaLocation)))
                OnGmlEvent(GeoDataEventType.SchemaOgcImportError, sourceFileInfo); // 5.G_8           

            if (!schema.Items.OfType<XmlSchemaAnnotation>().ToList().Exists(x => !x.IsEmpty())) OnGmlEvent(GeoDataEventType.SchemaMissingAnnotation, sourceFileInfo); // 5.G_12

            var geometri = schema.Elements.Values.OfType<XmlSchemaElement>().ToList().Find(x => x.QualifiedName.Name == "GEOMETRI");
            if (geometri != null)
            {
                if (!geometri.SubstitutionGroup.Name.Equals("_Feature")
                    || !geometri.SubstitutionGroup.Namespace.Equals(Constants.OpenGisNs))
                    OnGmlEvent(GeoDataEventType.SchemaGeometrySubstitutionGroupError, sourceFileInfo); // 5.G_11 
            }
            else
            {
                OnGmlEvent(GeoDataEventType.SchemaGeometryMissingError, sourceFileInfo); // 5.G_9
            }

            var complexTypes = schema.SchemaTypes.Values.OfType<XmlSchemaComplexType>().ToList();
            if (!complexTypes.Exists(x => x.ContentModel.Content is XmlSchemaComplexContentExtension
                                          && (x.ContentModel.Content as XmlSchemaComplexContentExtension).BaseTypeName.Name.Equals("AbstractFeatureType")
                                          && (x.ContentModel.Content as XmlSchemaComplexContentExtension).BaseTypeName.Namespace.Equals(Constants.OpenGisNs)))
                OnGmlEvent(GeoDataEventType.SchemaExtensionBaseError, sourceFileInfo); // 5.G_13


            var sequence =
                complexTypes.Find(
                    x =>
                        x.ContentModel.Content is XmlSchemaComplexContentExtension
                        && (x.ContentModel.Content as XmlSchemaComplexContentExtension).Particle is XmlSchemaSequence);

            var content =
                (sequence.ContentModel.Content as XmlSchemaComplexContentExtension).Particle as XmlSchemaSequence;


            var elements = content.Items.OfType<XmlSchemaElement>().ToList();

            var features = elements.FindAll(x => !string.IsNullOrEmpty(x.Name));
            if (!features.Any()) OnGmlEvent(GeoDataEventType.SchemaMissingFeature, sourceFileInfo); // 5.G_15

            if (features.Exists(x => x.Annotation.IsEmpty())) OnGmlEvent(GeoDataEventType.SchemaMissingFeatureAnnotation, sourceFileInfo); // 5.G_16             

            if (!elements.Exists(x => x.RefName.Namespace.Equals(Constants.OpenGisNs))) OnGmlEvent(GeoDataEventType.SchemaMissingOgcRef, sourceFileInfo); // 5.G_17
        }

        public bool FeatureHasGeometry(XElement featureMember)
        {
            var hasGeometry = false;
            var validGeometries = new List<string> {"posList", "pos", "coordinates"};

            if (featureMember != null)
            {
                var geometries =
                    featureMember.Descendants()
                        .ToList()
                        .FindAll(
                            x =>
                                x.Name.Namespace.Equals(OpenGisNs) && validGeometries.Contains(x.Name.LocalName));

                foreach (var geometry in geometries)
                {
                    hasGeometry = true;
                    if (currentDimensions.HasValue && currentBounds != null)
                    {
                        var coordinates = Point.PointListFromString(geometry.Value, currentDimensions.Value);
                        foreach (var point in coordinates)
                            if (!point.IsWithin(currentBounds))
                                OnGmlEvent(GeoDataEventType.GmlGeometryOutOfBounds, currentXmlStream.file); // 5.G_31
                    }
                }
            }

            return hasGeometry;
        }


        private XmlSchemaSet GetBaseSchemas()
        {
            if (baseSchemaSet.Count.Equals(0))
                if (baseSchemaStreams != null && baseSchemaStreams.Any())
                {
                    foreach (var stream in baseSchemaStreams)
                    {
                        currentSchemaStream = stream;
//                        stream.Position = 0;
                        LoadOfflineSchema(baseSchemaSet, stream);
                    }

                    baseSchemaSet.Compile();
                }

            return baseSchemaSet;
        }

        protected override XmlSchemaSet LoadSchemas(BufferedProgressStream schemaStream)
        {
            var schemas = new XmlSchemaSet {XmlResolver = null};
            schemas.Add(GetBaseSchemas());
//            schemas.Compile();
            schemas.ValidationEventHandler += SchemaValidationEventHandler;
            //            XmlSchemaSet schemas = this.GetBaseSchemas();
            if (schemaStream != null)
            {
                currentSchemaStream = schemaStream;
                LoadOfflineSchema(schemas, schemaStream);
                localschema = schemas.Schemas().OfType<XmlSchema>().Last();
                CheckSchema(localschema, currentSchemaStream.file);
            }

            schemas.Compile();
            return schemas;
        }

        private bool LocalSchemaLocationOk(List<XAttribute> attributes)
        {
            var xsiLocationprefix = XNamespace.Get(Constants.XsiNs) + Constants.SchemaLocationName;
            return attributes.Exists(x => x.Name.Equals(xsiLocationprefix) && x.Value.Equals(ExpectedSchemaLocation));
        }

        private bool NameSpaceAttributesOk(List<XAttribute> attributes)
        {
            var xlinkNsPrefix = XNamespace.Xmlns + Constants.XlinkNs.Split('/').Last();
            var openGisNsPrefix = XNamespace.Xmlns + Constants.OpenGisNs.Split('/').Last();
            var xsiNsPrefix = XNamespace.Xmlns + "xsi";
            var localNsPrefix = XNamespace.Xmlns + localschema.TargetNamespace.Split('/').Last();

            return attributes.Exists(x => x.Name.Equals(xlinkNsPrefix) && x.Value.Equals(Constants.XlinkNs))
                   && attributes.Exists(x => x.Name.Equals(openGisNsPrefix) && x.Value.Equals(Constants.OpenGisNs))
                   && attributes.Exists(x => x.Name.Equals(xsiNsPrefix) && x.Value.Equals(Constants.XsiNs))
                   && attributes.Exists(x => x.Name.Equals(localNsPrefix) && x.Value.Equals(localschema.TargetNamespace));
        }

        protected virtual void OnGmlEvent(GeoDataEventType eventType, FileInfo fileInfo)
        {
            var args = new GeoDataEventArgs(eventType, fileInfo);
            var eventHandler = GeoDataEvent;
            if (eventHandler != null) eventHandler(this, args);
        }


        protected override void ReadAll()
        {
            CheckRootCollection();
            var hasFeatures = false;
            foreach (var feature in FeatureStream())
            {
                hasFeatures = true;
                if (!FeatureHasGeometry(feature)) OnGmlEvent(GeoDataEventType.GmlNoGeometryError, currentXmlStream.file); // 5.G_30 
            }

            if (!hasFeatures) OnGmlEvent(GeoDataEventType.GmlFeatureMemberNotFound, currentXmlStream.file); // 5.G_29
        }

        #endregion
    }
}