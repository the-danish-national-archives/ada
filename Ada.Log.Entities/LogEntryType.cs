namespace Ada.Log.Entities
{
    public class LogEntryType
    {
        #region Properties

        public virtual string EntryText { get; set; }
        public virtual string EntryTypeId { get; set; }
        public virtual int Severity { get; set; }

        #endregion
    }
}