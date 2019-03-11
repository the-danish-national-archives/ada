// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The column map.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.EntityMappings.NHibernate.TableIndex
{
    #region Namespace Using

    using System.Diagnostics.CodeAnalysis;
    using DomainEntities.TableIndex;
    using FluentNHibernate.Mapping;
    using global::NHibernate.Type;

    #endregion

    /// <summary>
    ///     Fluent NHibernate mapping for the Column Entity.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ColumnMap : ClassMap<Column>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColumnMap" /> class.
        /// </summary>
        public ColumnMap()
        {
            Id(c => c.Key).Column("columnKey").GeneratedBy.Native();
            References(c => c.ParentTable).Column("tableKey").ForeignKey("tableKey");
            Map(c => c.TableName).Column("tableName");
            Map(c => c.ColumnId).Column("colNumber");
            Map(c => c.Name).Column("colName");
            Map(c => c.Description).Column("colDescription");
            Map(c => c.DefaultValue).Column("defaultValue");
            Map(c => c.TypeOriginal).Column("colOriginalType");
            Map(c => c.Type).Column("colType");
            Map(c => c.Nullable).Column("nullAble");

            HasMany(c => c.FunctionalDescriptions).KeyColumn("columnKey").Table("functionalDescription").Element("functionalDescription", e => e.Type<EnumStringType<FunctionalDescription>>()).Cascade.All();
            Table("columns");
        }

        #endregion
    }
}