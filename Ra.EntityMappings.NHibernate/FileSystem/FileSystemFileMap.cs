namespace Ra.EntityMappings.NHibernate.FileSystem
{
    #region Namespace Using

    using DomainEntities.FileSystem;
    using FluentNHibernate.Mapping;

    #endregion

    public class FileSystemFileMap : SubclassMap<FileSystemFile>
    {
        #region  Constructors

        public FileSystemFileMap()
        {
            Map(c => c.Extension).Column("extension");
            Map(c => c.Size).Column("size");
            Map(c => c.CheckSum).Column("checkSum");

            DiscriminatorValue(@"File");
        }

        #endregion
    }
}