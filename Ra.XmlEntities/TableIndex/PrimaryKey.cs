// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryKey.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The primary key element from a TableIndex.
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
    ///     The primary key element from a TableIndex.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "primaryKey")]
    public class PrimaryKey
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        [XmlElement("column", DataType = "token")]
        public List<string> Columns { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        #endregion
    }
}