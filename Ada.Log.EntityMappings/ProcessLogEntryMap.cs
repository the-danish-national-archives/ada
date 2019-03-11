namespace Ada.Log.EntityMappings
{
    #region Namespace Using

    using Entities;
    using FluentNHibernate.Mapping;

    #endregion

    public class ProcessLogEntryMap : ClassMap<ProcessLogEntry>
    {
        #region  Constructors

        public ProcessLogEntryMap()
        {
            Id(c => c.Key).Column("key").GeneratedBy.Native();
            Map(c => c.InternalName).Column("withTarget");
            Map(c => c.Type).Column("type");
            Map(c => c.StartTime).Column("startTime");
            Map(c => c.StopTime).Column("stopTime");
            Map(c => c.Duration).Column("duration");
            References(c => c.Parent).Column("parentId");
            Table("processLogEntries");
        }

        #endregion
    }
}