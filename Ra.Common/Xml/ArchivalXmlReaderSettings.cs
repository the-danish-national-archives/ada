// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchivalXmlReaderSettings.cs" company="">
//   
// </copyright>
// <summary>
//   The archival xml reader settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System.Text;

    #endregion

    /// <summary>
    ///     The archival xml reader settings.
    /// </summary>
    public class ArchivalXmlReaderSettings
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArchivalXmlReaderSettings" /> class.
        /// </summary>
        /// <param name="requireXmlProlog">
        ///     The require xml prolog.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        public ArchivalXmlReaderSettings(bool requireXmlProlog = true, Encoding encoding = null)
        {
            RequireXmlProlog = requireXmlProlog;
            Encoding = encoding ?? Encoding.UTF8;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the encoding.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether require xml prolog.
        /// </summary>
        public bool RequireXmlProlog { get; }

        #endregion
    }
}