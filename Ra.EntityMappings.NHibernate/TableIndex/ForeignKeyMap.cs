// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForeignKeyMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The foreign key map.
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
    ///     Fluent NHibernate mapping for the ForeignKey Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ForeignKeyMap : SubclassMap<ForeignKey>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ForeignKeyMap" /> class.
        /// </summary>
        public ForeignKeyMap()
        {
            HasMany(c => c.References).Inverse().Cascade.AllDeleteOrphan();
            Map(c => c.ReferencedTable).Column("referencedTable");
            DiscriminatorValue(@"FK");
        }

        #endregion
    }
}