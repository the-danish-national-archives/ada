// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForeignKey.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The foreign key element from a TableIndex.
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
    ///     The foreign key element from a TableIndex.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "foreignKey")]
    public class ForeignKey
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the referenced table.
        /// </summary>
        [XmlElement("referencedTable")]
        public string ReferencedTable { get; set; }

        /// <summary>
        ///     Gets or sets the references.
        /// </summary>
        [XmlElement("reference")]
        public List<Reference> References { get; set; }

        #endregion
    }
}