namespace Ra.DocumentInvestigator.NHibernate
{
    #region Namespace Using

    using AdaAvChecking.Image;
    using FluentNHibernate.Mapping;

    #endregion

    public class PageMap : ClassMap<PageObject>
    {
        #region  Constructors

        public PageMap()
        {
            Table("TiffPages");
            Id(x => x.PageId);
            Map(x => x.Compression);
            Map(x => x.OddDBI);
            Map(x => x.Fillorder);
            Map(x => x.PageNo);
            Map(x => x.PrivateTagsInPage);
            Map(x => x.BlankDokumentPage);
            References(x => x.TiffFileID);
        }

        #endregion
    }
}