namespace Ra.EntityMappings.NHibernate.ArchiveIndex
{
    #region Namespace Using

    using DomainEntities.ArchiveIndex;
    using FluentNHibernate.Mapping;

    #endregion

    internal class FormClassMap : ClassMap<FormClass>
    {
        #region  Constructors

        public FormClassMap()
        {
            Id(c => c.Key).Column("formClassKey").GeneratedBy.Native();
            Map(c => c.Class).Column("class");
            Map(c => c.Text).Column("text");
            References(c => c.Form).Column("formKey");
            Table("formClasses");
        }

        #endregion
    }
}