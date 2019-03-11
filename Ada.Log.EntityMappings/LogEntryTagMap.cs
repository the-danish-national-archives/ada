namespace Ada.Log.EntityMappings
{
    #region Namespace Using

    using Entities;
    using FluentNHibernate.Mapping;

    #endregion

    public class LogEntryTagMap : ClassMap<LogEntryTag>
    {
        #region  Constructors

        public LogEntryTagMap()
        {
            Id(c => c.TagId).Column("tagId").GeneratedBy.Native();
            Map(c => c.TagType).Column("tagType");
            Map(c => c.TagText).Column("tagText");
            References(c => c.ParentEntry).Column("entryId");
            Table("entryTags");
        }

        #endregion
    }
}