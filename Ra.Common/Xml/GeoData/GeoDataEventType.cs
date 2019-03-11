namespace Ra.Common.Xml.GeoData
{
    public enum GeoDataEventType
    {
        SchemaOgcImportError,
        SchemaGeometryMissingError,
        SchemaGeometrySubstitutionGroupError,
        SchemaExtensionBaseError,
        SchemaMissingAnnotation,
        SchemaMissingOgcRef,
        SchemaMissingFeature,
        SchemaMissingFeatureAnnotation,
        GmlSchemaLocationError,
        GmlRootElementError,
        GmlNameSpaceDeclarationerror,
        GmlIllegalEPSG,
        GmlIllegalDimension,
        GmlIllegalBounds,
        GmlFeatureMemberNotFound,
        GmlNoGeometryError,
        GmlGeometryOutOfBounds
    }
}