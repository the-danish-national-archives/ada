namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class DocumentCategoryTypeMap : ClassMap<DocumentCategoryType>
    {
        #region  Constructors

        public DocumentCategoryTypeMap()
        {
            Id(c => c.Key).Column("categoryTypeKey").GeneratedBy.Native();

            HasOne(c => c.ArchivalPreservationInformation).PropertyRef(r => r.Category).Cascade.All().Class<ArchivalPreservationInformationType>();
            HasOne(c => c.InformationOther).PropertyRef(r => r.Category).Cascade.All().Class<InformationOtherType>();
            HasOne(c => c.IngestInformation).PropertyRef(r => r.Category).Cascade.All().Class<IngestInformationType>();
            HasOne(c => c.OperationalInformation).PropertyRef(r => r.Category).Cascade.All().Class<OperationalInformationType>();
            HasOne(c => c.SubmissionInformation).PropertyRef(r => r.Category).Cascade.All().Class<SubmissionInformationType>();
            HasOne(c => c.SystemInformation).PropertyRef(r => r.Category).Cascade.All().Class<SystemInformationType>();

            References(c => c.Document).Column("documentKey").ForeignKey("documentKey");
            Table("documentCategoryTypes");
        }

        #endregion
    }
}