namespace Ada.Log.EntityMappings
{
    #region Namespace Using

    using Entities;
    using FluentNHibernate.Mapping;

    #endregion

    internal class LogDbInfoMap : ClassMap<LogDbInfo>
    {
        #region  Constructors

        public LogDbInfoMap()
        {
            Id(c => c.LogDbInfoKey).Column("key").GeneratedBy.Native();
            Map(c => c.Version).Column("Version");
        }

        #endregion
    }
}