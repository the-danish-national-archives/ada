// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionalDescription.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The list of EO 1007 functional descriptions, detailing the contents of a field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.XmlEntities.TableIndex
{
    #region Namespace Using

    using System;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The list of EO1007 functional descriptions, detailing the contents of a field.
    /// </summary>
    [Serializable]
    [XmlType("functionalDescription", Namespace = "http://www.sa.dk/xmlns/diark/1.0")]
    public enum FunctionalDescription
    {
        /// <summary>
        ///     Authority/entity id
        /// </summary>
        Myndighedsidentifikation,

        /// <summary>
        ///     Document id
        /// </summary>
        Dokumentidentifikation,

        /// <summary>
        ///     The lagringsform.
        /// </summary>
        Lagringsform,

        /// <summary>
        ///     Submitted.
        /// </summary>
        Afleveret,

        /// <summary>
        ///     Case id
        /// </summary>
        Sagsidentifikation,

        /// <summary>
        ///     Case title
        /// </summary>
        Sagstitel,

        /// <summary>
        ///     Document title
        /// </summary>
        Dokumenttitel,

        /// <summary>
        ///     Document date.
        /// </summary>
        Dokumentdato,

        /// <summary>
        ///     The afsender_modtager.
        /// </summary>
        Afsender_modtager,

        /// <summary>
        ///     Digital Signature.
        /// </summary>
        Digital_signatur,

        /// <summary>
        ///     FORM classification code.
        /// </summary>
        FORM,

        /// <summary>
        ///     Discarded.
        /// </summary>
        Kassation
    }
}