// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="">
//   
// </copyright>
// <summary>
//   The reference element from a TableIndex.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.TableIndex
{
    using System.Xml.Serialization;

    /// <summary>
    /// The reference element from a TableIndex.
    /// </summary>
    [XmlRoot(ElementName = "reference")]
    public class Reference
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the constraint column.
        /// </summary>
        [XmlElement("column")]
        public string Column { get; set; }

        /// <summary>
        /// Gets or sets the referenced column.
        /// </summary>
        [XmlElement("referenced")]
        public string Referenced { get; set; }

        #endregion
    }
}