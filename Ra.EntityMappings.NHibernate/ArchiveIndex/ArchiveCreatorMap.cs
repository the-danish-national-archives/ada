namespace Ra.EntityMappings.NHibernate.ArchiveIndex
{
    #region Namespace Using

    using DomainEntities.ArchiveIndex;
    using FluentNHibernate.Mapping;

    #endregion

    public class ArchiveCreatorMap : ClassMap<ArchiveCreator>
    {
        #region  Constructors

        public ArchiveCreatorMap()
        {
            Id(c => c.Key).Column("archiveCreatorKey").GeneratedBy.Native();
            Map(c => c.CreatorName).Column("creatorName");
            Map(c => c.CreationPeriodStart).Column("creationPeriodStart");
            Map(c => c.CreationPeriodEnd).Column("creationPeriodEnd");
            References(c => c.ArchiveIndex).Column("archiveIndexKey");
            Table("archiveCreators");
        }

        #endregion
    }
}