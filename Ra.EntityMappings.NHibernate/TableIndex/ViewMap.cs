// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewMap.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   The view map.
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
    ///     Fluent NHibernate mapping for the View Entity.
    /// </summary>
    public class ViewMap : ClassMap<View>
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewMap" /> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public ViewMap()
        {
            Id(c => c.Key).Column("viewKey").GeneratedBy.Native();
            Map(c => c.Name).Column("viewName");
            Map(c => c.QueryOriginal).Column("view");
            Map(c => c.Description).Column("description");
            References(c => c.TableIndex).Column("tableIndexKey");
            Table("views");
        }

        #endregion
    }
}