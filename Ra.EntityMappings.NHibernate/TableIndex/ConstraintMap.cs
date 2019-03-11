// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The constraint map.
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
    ///     Fluent NHibernate mapping for the Constraint Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ConstraintMap : ClassMap<Constraint>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConstraintMap" /> class.
        /// </summary>
        public ConstraintMap()
        {
            Id(c => c.Key).Column("constraintKey").GeneratedBy.Native();
            Map(c => c.Name).Column("constraintName");
            Map(c => c.ParentTableName).Column("tableName");
            References(c => c.ParentTable).Column("tableKey");
            HasMany(c => c.ConstraintColumns).Inverse().Cascade.AllDeleteOrphan();
            Table("constraintNames");


            DiscriminateSubClassesOnColumn("constraintType").AlwaysSelectWithValue();
        }

        #endregion
    }
}