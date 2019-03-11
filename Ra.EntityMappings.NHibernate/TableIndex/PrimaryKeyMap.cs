// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryKeyMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The primary key map.
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
    ///     Fluent NHibernate mapping for the PrimaryKey Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class PrimaryKeyMap : SubclassMap<PrimaryKey>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PrimaryKeyMap" /> class.
        /// </summary>
        public PrimaryKeyMap()
        {
            DiscriminatorValue(@"PK");
            HasMany(x => x.Columns).KeyColumn("constraintKey").Table("newConstraintColumns").Element("columnName");
        }

        #endregion
    }
}