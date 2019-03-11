namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class IngestInformationTypeMap : ClassMap<IngestInformationType>
    {
        #region  Constructors

        public IngestInformationTypeMap()
        {
            Id(c => c.Key).Column("ingestInformationTypeKey").GeneratedBy.Native();
            Map(c => c.ArchivalInformationOther).Column("archivalInformationOther");
            Map(c => c.ArchivalTestNotes).Column("archivalTestNotes");
            Map(c => c.ArchivistNotes).Column("archivistNotes");

            References(c => c.Category).Column("categoryTypeKey").ForeignKey("categoryTypeKey");
            Table("ingestInformationTypes");
        }

        #endregion
    }
}