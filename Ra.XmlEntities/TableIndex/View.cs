// --------------------------------------------------------------------------------------------------------------------
// <copyright file="View.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The view element from a TableIndex.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.TableIndex
{
    #region Namespace Using

    using System;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The view element from a TableIndex.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false, ElementName = "view")]
    public class View
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the view description.
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [XmlElement("name", DataType = "token")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the original query.
        /// </summary>
        [XmlElement("queryOriginal")]
        public string OriginalQuery { get; set; }

        #endregion
    }
}