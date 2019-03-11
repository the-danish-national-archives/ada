// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintColumnMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The constraint column map.
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
    ///     Fluent NHibernate mapping for the ConstraintColumn Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ConstraintColumnMap : ClassMap<ConstraintColumn>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConstraintColumnMap" /> class.
        /// </summary>
        public ConstraintColumnMap()
        {
            Id(c => c.Key).Column("constraintColumnKey").GeneratedBy.Native();
            References(c => c.Constraint).Column("constraintKey");
            Map(c => c.ConstraintName).Column("constraintName");
            Map(c => c.Column).Column("constraintColumnName");
            Map(c => c.TableName).Column("tableName");
            Table("constraintColumns");
        }

        #endregion
    }
}