namespace Ada.Log.Entities
{
    public class LogEntryTag
    {
        #region Properties

        public virtual LogEntry ParentEntry { get; set; }
        public virtual int TagId { get; set; }
        public virtual string TagText { get; set; }
        public virtual string TagType { get; set; }

        #endregion
    }
}