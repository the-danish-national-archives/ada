namespace Ada.Log.Entities
{
    #region Namespace Using

    using System;

    #endregion

    public class ProcessLogEntry
    {
        #region Properties

        public virtual int Duration { get; set; }
        public virtual string InternalName { get; set; }
        public virtual int Key { get; set; }

        public virtual ProcessLogEntry Parent { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? StopTime { get; set; }
        public virtual string Type { get; set; }

        #endregion
    }
}