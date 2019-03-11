// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The reference map.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.EntityMappings.NHibernate.TableIndex
{
    #region Namespace Using

    using System.Diagnostics.CodeAnalysis;
    using DomainEntities.TableIndex;
    using FluentNHibernate.Mapping;

    #endregion

    /// <summary>
    ///     Fluent NHibernate mapping for the Reference Entity.
    /// </summary>
    public class ReferenceMap : ClassMap<Reference>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReferenceMap" /> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public ReferenceMap()
        {
            Id(c => c.Key).Column("referenceKey").GeneratedBy.Native();
            Map(c => c.Column).Column("constraintColumn");
            Map(c => c.Referenced).Column("referencedColumn");
            References(c => c.ParentConstraint).Column("constraintKey");
            Table("newReferenceColumns");
        }

        #endregion
    }
}