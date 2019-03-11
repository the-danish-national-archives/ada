namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class OperationalInformationTypeMap : ClassMap<OperationalInformationType>
    {
        #region  Constructors

        public OperationalInformationTypeMap()
        {
            Id(c => c.Key).Column("operationalInformationTypeKey").GeneratedBy.Native();
            Map(c => c.OperationalSystemConvertedInformation).Column("operationalSystemConvertedInformation");
            Map(c => c.OperationalSystemInformation).Column("operationalSystemInformation");
            Map(c => c.OperationalSystemInformationOther).Column("operationalSystemInformationOther");
            Map(c => c.OperationalSystemSOA).Column("operationalSystemSOA");

            References(c => c.Category).Column("categoryTypeKey").ForeignKey("categoryTypeKey");
            Table("operationalInformationTypes");
        }

        #endregion
    }
}