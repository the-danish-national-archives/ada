namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class SystemInformationTypeMap : ClassMap<SystemInformationType>
    {
        #region  Constructors

        public SystemInformationTypeMap()
        {
            Id(c => c.Key).Column("SystemInformationTypeKey").GeneratedBy.Native();

            Map(c => c.SystemAdministrativeFunctions).Column("systemAdministrativeFunctions");
            Map(c => c.SystemAgencyQualityControl).Column("systemAgencyQualityControl");
            Map(c => c.SystemContent).Column("systemContent");
            Map(c => c.SystemDataProvision).Column("systemDataProvision");
            Map(c => c.SystemDataTransfer).Column("systemDataTransfer");
            Map(c => c.SystemInformationOther).Column("systemInformationOther");
            Map(c => c.SystemPresentationStructure).Column("systemPresentationStructure");
            Map(c => c.SystemPreviousSubsequentFunctions).Column("systemPreviousSubsequentFunctions");
            Map(c => c.SystemPublication).Column("systemPublication");
            Map(c => c.SystemPurpose).Column("systemPurpose");
            Map(c => c.SystemRegulations).Column("systemRegulations");

            References(c => c.Category).Column("categoryTypeKey").ForeignKey("categoryTypeKey");
            Table("systemInformationTypes");
        }

        #endregion
    }
}