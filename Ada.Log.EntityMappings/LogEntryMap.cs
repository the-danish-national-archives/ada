namespace Ada.Log.EntityMappings
{
    #region Namespace Using

    using Entities;
    using FluentNHibernate.Mapping;

    #endregion

    public class LogEntryMap : ClassMap<LogEntry>
    {
        #region  Constructors

        public LogEntryMap()
        {
            Id(c => c.EntryId).Column("entryID").GeneratedBy.Native();
            Map(c => c.EntryTypeId).Column("entryTypeId");
            Map(c => c.CheckName).Column("checkName");
            Map(c => c.FormattedText).Column("formattedText");
            HasMany(c => c.EntryTags).Inverse().Cascade.AllDeleteOrphan();
            Map(c => c.TimeStamp).Column("timeStamp");
            Map(c => c.SessionKey).Column("sessionKey");
            References(c => c.OwnerProcess).Column("ownerId");
            Table("logEntries");
        }

        #endregion
    }
}