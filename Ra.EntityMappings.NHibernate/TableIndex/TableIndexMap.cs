// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableIndexMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The table index map.
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
    ///     Fluent NHibernate mapping for the TableIndex Entity.
    /// </summary>
    public class TableIndexMap : ClassMap<TableIndex>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TableIndexMap" /> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public TableIndexMap()
        {
            Id(c => c.Key).Column("tableIndexKey").GeneratedBy.Native();
            Map(c => c.DbName).Column("dbName");
            Map(c => c.DbProduct).Column("dbProduct");
            Map(c => c.Version).Column("version");
            Map(c => c.SessionKey).Column("sessionKey");
            HasMany(c => c.Tables).Inverse().Cascade.AllDeleteOrphan();
            HasMany(c => c.Views).Inverse().Cascade.AllDeleteOrphan();
            Table("tableIndex");
        }

        #endregion
    }
}