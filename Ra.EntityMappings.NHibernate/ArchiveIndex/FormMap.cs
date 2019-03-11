namespace Ra.EntityMappings.NHibernate.ArchiveIndex
{
    #region Namespace Using

    using DomainEntities.ArchiveIndex;
    using FluentNHibernate.Mapping;

    #endregion

    internal class FormMap : ClassMap<Form>
    {
        #region  Constructors

        public FormMap()
        {
            Id(c => c.Key).Column("formKey").GeneratedBy.Native();
            Map(c => c.FormVersion).Column("formVersion");
            References(c => c.ArchiveIndex).Column("archiveIndexKey");
            HasMany(c => c.Classes).Cascade.All().Inverse();
            Table("form");
        }

        #endregion
    }
}