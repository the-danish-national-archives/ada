// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotatedEntity.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The AnnotatedEntity interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.DomainEntities
{
    /// <summary>
    ///     The AnnotatedEntity interface.
    /// </summary>
    public interface IAnnotatedEntity
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        string Description { get; }

        #endregion
    }
}