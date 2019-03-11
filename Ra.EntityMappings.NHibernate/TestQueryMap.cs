namespace Ada.EntityMaps
{
    #region Namespace Using

    using Entities;
    using FluentNHibernate.Mapping;

    #endregion

    public class TestQueryMap : ClassMap<TestQuery>
    {
        #region  Constructors

        public TestQueryMap()
        {
            Id(c => c.QueryKey).Column("queryKey").GeneratedBy.Native();
            Map(c => c.TestId).Column("testID");
            Map(c => c.Query).Column("query");
            Map(c => c.Description).Column("desc");
            Table("testQueries");
        }

        #endregion
    }
}