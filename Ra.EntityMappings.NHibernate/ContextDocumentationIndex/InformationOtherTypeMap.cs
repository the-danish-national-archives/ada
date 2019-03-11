namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class InformationOtheTypeMap : ClassMap<InformationOtherType>
    {
        #region  Constructors

        public InformationOtheTypeMap()
        {
            Id(c => c.Key).Column("informationOtherTypeKey").GeneratedBy.Native();

            Map(c => c.InformationOther).Column("informationOther");

            References(c => c.Category).Column("categoryTypeKey").ForeignKey("categoryTypeKey");
            Table("informationOtherTypes");
        }

        #endregion
    }
}