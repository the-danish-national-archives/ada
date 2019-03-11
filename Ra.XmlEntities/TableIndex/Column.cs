// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Column.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The column element from a TableIndex.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.TableIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The column element from a TableIndex.
    /// </summary>
    [Serializable]
    [XmlType("column", Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class Column
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the column id.
        /// </summary>
        [XmlElement("columnID")]
        public string ColumnId { get; set; }

        /// <summary>
        ///     Gets or sets the default value.
        /// </summary>
        [XmlElement("defaultValue")]
        public string DefaultValue { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the functional descriptions.
        /// </summary>
        [XmlElement("functionalDescription")]
        public List<FunctionalDescription> FunctionalDescriptions { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [XmlElement("name", DataType = "token")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the field is nullable.
        /// </summary>
        [XmlElement("nullable")]
        public bool Nullable { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        [XmlElement("type", DataType = "token")]
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the original type.
        /// </summary>
        [XmlElement("typeOriginal", DataType = "token")]
        public string TypeOriginal { get; set; }

        #endregion
    }
}