// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocIndex.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//    XML serializable entity corresponding to the EO1007 document index xml file.
//    Included for completeness and test cases. Only for use with small collections. Not suitable in large scale production scenarios. Not used by ADA.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.DocIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The EO1007 document index.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot("docIndex", Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false)]
    public class DocIndex
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the collection of documents.
        /// </summary>
        [XmlElement("doc")]
        public List<Document> Documents { get; set; }

        #endregion
    }
}