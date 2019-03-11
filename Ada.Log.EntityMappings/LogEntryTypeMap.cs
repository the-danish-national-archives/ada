namespace Ada.Log.EntityMappings
{
    #region Namespace Using

    using Entities;
    using FluentNHibernate.Mapping;

    #endregion

    public class LogEntryTypeMap : ClassMap<LogEntryType>
    {
        #region  Constructors

        public LogEntryTypeMap()
        {
            Id(c => c.EntryTypeId).Column("entryTypeId");
            Map(c => c.EntryText).Column("entryText");
            Map(c => c.Severity).Column("severity");
            Table("entryTypes");
        }

        #endregion
    }
}