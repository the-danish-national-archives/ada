namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class ContextDocumentationDocumentAuthorMap : ClassMap<ContextDocumentationIndexDocumentAuthor>
    {
        #region  Constructors

        public ContextDocumentationDocumentAuthorMap()
        {
            Id(c => c.Key).Column("authorKey").GeneratedBy.Native();
            Map(c => c.AuthorName).Column("authorName");
            Map(c => c.AuthorInstitution).Column("authorInstitution");
            References(c => c.Document).Column("documentKey").ForeignKey("documentKey");
            Table("documentAuthor");
        }

        #endregion
    }
}