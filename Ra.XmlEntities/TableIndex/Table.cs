// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Table.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The table element from a TableIndex.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The table element from a TableIndex.
    /// </summary>
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "table")]
    public class Table
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        [XmlArray("columns", IsNullable = false)]
        [XmlArrayItem("column", IsNullable = false)]
        public List<Column> Columns { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the name of the folder containing the table xml
        /// </summary>
        [XmlElement("folder")]
        public string Folder { get; set; }

        /// <summary>
        ///     Gets or sets the foreign keys.
        /// </summary>
        [XmlArray("foreignKeys")]
        [XmlArrayItem(ElementName = "foreignKey")]
        public List<ForeignKey> ForeignKeys { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [XmlElement("name", DataType = "token")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the primary key.
        /// </summary>
        [XmlElement(ElementName = "primaryKey")]
        public PrimaryKey PrimaryKey { get; set; }

        /// <summary>
        ///     Gets or sets the expected number of rows.
        /// </summary>
        [XmlElement("rows", DataType = "nonNegativeInteger")]
        public string Rows { get; set; }

        #endregion
    }
}