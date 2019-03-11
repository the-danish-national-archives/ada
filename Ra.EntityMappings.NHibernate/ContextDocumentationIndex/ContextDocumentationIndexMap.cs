namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class ContextDocumentationIndexMap : ClassMap<ContextDocumentationIndex>
    {
        #region  Constructors

        public ContextDocumentationIndexMap()
        {
            Id(c => c.Key).Column("indexKey").GeneratedBy.Native();
            HasMany(c => c.Documents).Inverse().Cascade.AllDeleteOrphan();
            Table("contextDocumentationIndex");
        }

        #endregion
    }
}