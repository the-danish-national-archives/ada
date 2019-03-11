namespace Ra.DocumentInvestigator.AdaAvChecking.Image
{
    public class PageObject : IPageObject
    {
        #region IPageObject Members

        public virtual int PageId { get; set; }
        public virtual string Compression { get; set; }

        public virtual bool OddDBI { get; set; }

        public virtual FillorderEnum Fillorder { get; set; }

        public virtual int PageNo { get; set; }

        public virtual bool PrivateTagsInPage { get; set; }

        public virtual bool? BlankDokumentPage { get; set; }
        public virtual string TiffFileID { get; set; }

        #endregion

        //    public void Save()
        //  {
        //TODO Beslut om vi skal gemme oplysningerne i DB
        //string sql = " INSERT INTO TiffPages (PageId,Compression,OddDBI,PageNo, PrivateTagsInPage,BlankDokumentPage,TiffFileID) VALUES(@PageId,@Compression,@OddDBI,@PageNo, @PrivateTagsInPage,@BlankDokumentPage,@TiffFileID)";
        //System.Data.IDbCommand cmd = Database.Connection.CreateCommand();
        //cmd.CommandText = sql;
        //cmd.CommandType = CommandType.Text;
        //IDbDataParameter p = cmd.CreateParameter();
        //p.ParameterName = "@PageId";
        //p.DbType = DbType.Int32;
        //p.Value = this.PageId;
        //cmd.Parameters.Add(p);

        //p = cmd.CreateParameter();
        //p.ParameterName = "@Compression";
        //p.DbType = DbType.String;
        //p.Value = this.Compression;
        //cmd.Parameters.Add(p);

        //p = cmd.CreateParameter();
        //p.ParameterName = "@OddDBI";
        //p.DbType = DbType.Boolean;
        //p.Value = this.OddDBI;
        //cmd.Parameters.Add(p);

        //p = cmd.CreateParameter();
        //p.ParameterName = "@PageNo";
        //p.DbType = DbType.Int32;
        //p.Value = this.PageNo;
        //cmd.Parameters.Add(p);

        //p = cmd.CreateParameter();
        //p.ParameterName = "@PrivateTagsInPage";
        //p.DbType = DbType.Boolean;
        //p.Value = this.PrivateTagsInPage;
        //cmd.Parameters.Add(p);

        //p = cmd.CreateParameter();
        //p.ParameterName = "@BlankDokumentPage";
        //p.DbType = DbType.String;
        //p.Value = this.BlankDokumentPage;
        //cmd.Parameters.Add(p);

        //p = cmd.CreateParameter();
        //p.ParameterName = "@TiffFileID";
        //p.DbType = DbType.String;
        //p.Value = this.TiffFileID;
        //cmd.Parameters.Add(p);
        //}
    }
}