namespace Ra.EntityMappings.NHibernate
{
    #region Namespace Using

    using DomainEntities;
    using FluentNHibernate.Mapping;

    #endregion

    //public class SchemaMap : ClassMap<AdaSchema>
    //{
    //    #region Constructors and Destructors


    //    public SchemaMap()
    //    {
    //        //UseUnionSubclassForInheritanceMapping();
    //        this.Id(c => c.Key).Column("schemaKey").GeneratedBy.Native();
    //        this.Map(c => c.FileName).Column("name");
    //        this.Map(c => c.CheckSum).Column("md5");
    //    }
    //    #endregion
    //}

    public class AdaGisSchemaMap : ClassMap<AdaGisSchema>
    {
        #region  Constructors

        public AdaGisSchemaMap()
        {
            Id(c => c.Key).Column("indexSchemasKey").GeneratedBy.Native();
            Map(c => c.FileName).Column("name");
            Map(c => c.CheckSum).Column("MD5");
            Table("gisSchemas");
        }

        #endregion
    }
}