namespace Ada.Actions
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ActionBase;
    using ChecksBase;
    using Common;
    using Ra.Common.Xml.GeoData;

    #endregion

    public class GeoDataEventLogger
    {
        #region  Fields

        private readonly Action<AdaAvGmlViolation> _callBack;

        private readonly AVMapping avMapping;

        private readonly Dictionary<GeoDataEventType, Type> _errorLookup;

        #endregion

        #region  Constructors

        public GeoDataEventLogger(Action<AdaAvGmlViolation> callBack, Dictionary<GeoDataEventType, Type> errorLookup, AVMapping avMapping)
        {
            _callBack = callBack;
            _errorLookup = errorLookup;
            this.avMapping = avMapping;
        }

        #endregion

        #region

        public void GeoDataEventHandler(object sender, GeoDataEventArgs e)
        {
            if (!_errorLookup.ContainsKey(e.EventType))
                throw new InvalidOperationException();


            _callBack(
                AdaAvGmlViolation.CreateInstance(
                    _errorLookup[e.EventType],
                    avMapping.GetRelativePath(e.SourceFileInfo)));

//
//            var logEntry = new LogEntry();
//            logEntry.AddTag("Path", this.avMapping.GetRelativePath(e.SourceFileInfo));

//            switch (e.EventType)
//            {
//                case GeoDataEventType.SchemaOgcImportError:
//                    logEntry.EntryTypeId = "5.G_8";
//                    break;
//                case GeoDataEventType.SchemaGeometryMissingError:
//                    logEntry.EntryTypeId = "5.G_9";
//                    break;
//                case GeoDataEventType.SchemaGeometrySubstitutionGroupError:
//                    logEntry.EntryTypeId = "5.G_10";
//                    break;
//                case GeoDataEventType.SchemaMissingAnnotation:
//                    logEntry.EntryTypeId = "5.G_11";
//                    break;
//                case GeoDataEventType.SchemaExtensionBaseError:
//                    logEntry.EntryTypeId = "5.G_12";
//                    break;
//                case GeoDataEventType.SchemaMissingFeature:
//                    logEntry.EntryTypeId = "5.G_13";
//                    break;
//                case GeoDataEventType.SchemaMissingFeatureAnnotation:
//                    logEntry.EntryTypeId = "5.G_14";
//                    break;
//                case GeoDataEventType.SchemaMissingOgcRef:
//                    logEntry.EntryTypeId = "5.G_17";
//                    break;
//                case GeoDataEventType.GmlSchemaLocationError:
//                    logEntry.EntryTypeId = "5.G_19";
//                    break;
//                case GeoDataEventType.GmlRootElementError:
//                    logEntry.EntryTypeId = "5.G_22";
//                    break;
//                case GeoDataEventType.GmlNameSpaceDeclarationerror:
//                    logEntry.EntryTypeId = "5.G_23";
//                    break;
//                case GeoDataEventType.GmlIllegalEPSG:
//                    logEntry.EntryTypeId = "5.G_24";
//                    break;
//                case GeoDataEventType.GmlIllegalDimension:
//                    logEntry.EntryTypeId = "5.G_25";
//                    break;
//                case GeoDataEventType.GmlIllegalBounds:
//                    logEntry.EntryTypeId = "5.G_26";
//                    break;
//                case GeoDataEventType.GmlFeatureMemberNotFound:
//                    logEntry.EntryTypeId = "5.G_27";
//                    break;
//                case GeoDataEventType.GmlNoGeometryError:
//                    logEntry.EntryTypeId = "5.G_28";
//                    break;
//                case GeoDataEventType.GmlGeometryOutOfBounds:
//                    logEntry.EntryTypeId = "5.G_29";
//                    break;
//            }

//            _callBack(logEntry);
            if (e.EventType != GeoDataEventType.SchemaMissingFeatureAnnotation) throw new AdaSkipActionException();
        }

        #endregion
    }
}