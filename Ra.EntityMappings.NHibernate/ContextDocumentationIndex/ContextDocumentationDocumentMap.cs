namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class ContextDocumentationDocumentMap : ClassMap<ContextDocumentationDocument>
    {
        #region  Constructors

        public ContextDocumentationDocumentMap()
        {
            Id(c => c.Key).Column("documentKey").GeneratedBy.Native();
            HasMany(c => c.Authors).KeyColumn("documentKey").Inverse().Cascade.AllDeleteOrphan();
            HasOne(c => c.DocumentCategory)
                .PropertyRef(r => r.Document)
                .Cascade.All()
                .Class<DocumentCategoryType>();
            Map(c => c.DocumentId).Column("dID");
            Map(c => c.DocumentTitle).Column("documentTitle");
            Map(c => c.SessionKey).Column("sessionKey");
            Map(c => c.archiveExtension).Column("archiveExtension");
            Map(c => c.MediaId).Column("medieNumber");
            Map(c => c.MaterializedPath).Column("materializedPath");
            Map(c => c.RelativePath).Column("relativePath");
            Map(c => c.DocumentDescription).Column("docDescription");
            Map(c => c.DocumentDate).Column("docDate");
            Table("contextDocumentationDocuments");
        }

        #endregion
    }
}