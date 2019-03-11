namespace Ra.EntityMappings.NHibernate.FileSystem
{
    #region Namespace Using

    using DomainEntities.FileSystem;
    using FluentNHibernate.Mapping;

    #endregion

    public class FileSystemEntryMap : ClassMap<FileSystemEntry>
    {
        #region  Constructors

        public FileSystemEntryMap()
        {
            Id(c => c.Key).Column("entryKey").GeneratedBy.Native();
            Map(c => c.Name).Column("name");
            Map(c => c.RelativePath).Column("relativePath");
            Map(c => c.MediaNumber).Column("mediaNumber");
            Map(c => c.Level).Column("depth");
            Map(c => c.TimeStamp).Column("timeStamp");
            HasManyToMany(x => x.Ancestors).ParentKeyColumn("descendant").ChildKeyColumn("ancestor").Table("fsEntryClosure");
            //this.HasMany(c => c.Ancestors).KeyColumn("entryKey").Table("fsEntryClosure");


            Table("fsEntries");

            DiscriminateSubClassesOnColumn("entryType").AlwaysSelectWithValue();
        }

        #endregion
    }
}