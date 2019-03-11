// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileIndex.cs" company="">
//   
// </copyright>
// <summary>
//    XML serializable entity corresponding to the EO1007 file index xml file.
//    Included for completeness and test cases. Only for use with small collections. Not suitable in large scale production scenarios. Not used by ADA.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.FileIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The file index.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    [XmlRoot("fileIndex", Namespace = "http://www.sa.dk/xmlns/diark/1.0", IsNullable = false)]
    public class FileIndex
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the collection of files.
        /// </summary>
        [XmlElement("f")]
        public List<FileIndexFile>[] Files { get; set; }

        #endregion
    }
}