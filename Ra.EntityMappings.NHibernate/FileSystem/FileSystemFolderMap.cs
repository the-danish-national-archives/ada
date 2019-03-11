namespace Ra.EntityMappings.NHibernate.FileSystem
{
    #region Namespace Using

    using DomainEntities.FileSystem;
    using FluentNHibernate.Mapping;

    #endregion

    public class FileSystemFolderMap : SubclassMap<FileSystemFolder>
    {
        #region  Constructors

        public FileSystemFolderMap()
        {
            Map(c => c.FileCount).Column("fileCount");
            Map(c => c.FolderCount).Column("folderCount");
            //this.HasMany(x => x.Ancestors).KeyColumn("entryKey").Table("fsEntryClosure").AsEntityMap();
            HasManyToMany(x => x.Contents).ParentKeyColumn("ancestor").ChildKeyColumn("descendant").Table("fsEntryClosure").Inverse();


            DiscriminatorValue(@"Dir");
        }

        #endregion
    }
}