namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class ArchivalPreservationInformationTypeMap
        : ClassMap<ArchivalPreservationInformationType>
    {
        #region  Constructors

        public ArchivalPreservationInformationTypeMap()
        {
            Id(c => c.Key).Column("archivalPreservationInformationkey").GeneratedBy.Native();

            Map(c => c.ArchivalInformationOther).Column("archivalInformationOther");
            Map(c => c.ArchivalMigrationInformation).Column("archivalMigrationInformation");

            References(c => c.Category).Column("categoryTypeKey").ForeignKey("categoryTypeKey");
            Table("archivalPreservationInformationTypes");
        }

        #endregion
    }
}