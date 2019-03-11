// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlEventType.cs" company="">
//   
// </copyright>
// <summary>
//   The xml event type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    /// <summary>
    ///     The xml event type.
    /// </summary>
    public enum XmlEventType
    {
        /// <summary>
        ///     The exception.
        /// </summary>
        Exception,
        XmlFileNotFound,
        SchemaFileNotFound,

        /// <summary>
        ///     The well formedness error.
        /// </summary>
        XmlWellFormednessError,

        /// <summary>
        ///     The validation error.
        /// </summary>
        XmlValidationError,

        /// <summary>
        ///     The validation warning.
        /// </summary>
        XmlValidationWarning,
        SchemaWellFormedNessError,
        SchemaValidationError,
        SchemaValidationWarning,
        XmlMissingProlog,
        XmlDeclaredEncodingIllegal,
        SchemaMissingProlog,
        SchemaDeclaredEncodingIllegal
    }
}