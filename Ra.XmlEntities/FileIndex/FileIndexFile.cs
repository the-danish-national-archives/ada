// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileIndexFile.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The file element from an EO1007 file index.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.FileIndex
{
    #region Namespace Using

    using System;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The file index file.
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public class FileIndexFile
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the digest.
        /// </summary>
        [XmlElement(DataType = "hexBinary")]
        [XmlElement("md5")]
        public byte[] Digest { get; set; }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        [XmlElement("fiN")]
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the folder name.
        /// </summary>
        [XmlElement("foN")]
        public string FolderName { get; set; }

        #endregion
    }
}