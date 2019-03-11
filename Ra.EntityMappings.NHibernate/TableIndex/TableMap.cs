// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The table map.
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
    ///     Fluent NHibernate mapping for the Table Entity.
    /// </summary>
    public class TableMap : ClassMap<Table>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TableMap" /> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public TableMap()
        {
            Id(c => c.Key).Column("tableKey").GeneratedBy.Native();
            Map(c => c.Name).Column("tableName");
            Map(c => c.Folder).Column("folder");
            Map(c => c.Description).Column("description");
            Map(c => c.Rows).Column("rows");
            HasOne(c => c.PrimaryKey).PropertyRef(r => r.ParentTable).Cascade.All().Class<PrimaryKey>();
            HasMany(c => c.ForeignKeys).KeyColumn("tableKey").Inverse().Cascade.AllDeleteOrphan();
            HasMany(c => c.Columns).Inverse().Cascade.AllDeleteOrphan();
            References(c => c.TableIndex).Column("tableIndexKey");
            Table("tables");
        }

        #endregion
    }
}