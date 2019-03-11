// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableIndex.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The table index entity
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The table index entity
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot("siardDiark", Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false)]
    public class TableIndex
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TableIndex" /> class.
        /// </summary>
        public TableIndex()
        {
            Version = "1.0";
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the name of the original database
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [XmlElement("dbName", DataType = "token")]
        public string DbName { get; set; }

        /// <summary>
        ///     Gets or sets the database product/DBMS data was extracted from
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [XmlElement("databaseProduct")]
        public string DbProduct { get; set; }

        /// <summary>
        ///     Gets or sets the tables.
        /// </summary>
        [XmlArray("tables")]
        [XmlArrayItem("table")]
        public List<Table> Tables { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        [XmlElement("version", DataType = "NMTOKEN")]
        public string Version { get; set; }

        /// <summary>
        ///     Gets or sets the views.
        /// </summary>
        [XmlArray("views")]
        [XmlArrayItem("view")]
        public List<View> Views { get; set; }

        #endregion
    }
}