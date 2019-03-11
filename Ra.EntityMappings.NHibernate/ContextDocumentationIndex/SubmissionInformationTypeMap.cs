namespace Ra.EntityMappings.NHibernate.ContextDocumentationIndex
{
    #region Namespace Using

    using DomainEntities.ContextDocumentationIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class SubmissionInformationTypeMap : ClassMap<SubmissionInformationType>
    {
        #region  Constructors

        public SubmissionInformationTypeMap()
        {
            Id(c => c.Key).Column("submissionInformationTypeKey").GeneratedBy.Native();
            Map(c => c.ArchivalInformationOther).Column("archivalInformationOther");
            Map(c => c.ArchivalProvisions).Column("archivalProvisions");
            Map(c => c.ArchivalTransformationInformation).Column("archivalTransformationInformation");

            References(c => c.Category).Column("categoryTypeKey").ForeignKey("categoryTypeKey");
            Table("submissionInformationTypes");
        }

        #endregion
    }
}