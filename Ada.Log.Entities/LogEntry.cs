namespace Ada.Log.Entities
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ra.Common.ExtensionMethods;

    #endregion

    public class LogEntry
    {
        #region  Constructors

        public LogEntry()
        {
            EntryTags = new List<LogEntryTag>();
            TimeStamp = $"{DateTime.Now:O}";
        }

        #endregion

        #region Properties

        public virtual string CheckName { get; set; }

        public virtual int EntryId { get; set; }
        public virtual IList<LogEntryTag> EntryTags { get; }
        public virtual string EntryTypeId { get; set; }
        public virtual string FormattedText { get; set; }

        public virtual ProcessLogEntry OwnerProcess { get; set; }
        public virtual Guid SessionKey { get; set; }
        public virtual int Severity { get; set; }
        public virtual string TimeStamp { get; set; }

        #endregion

        #region

        public virtual void AddTag(LogEntryTagType tagType, string tagContent)
        {
            AddTag(tagType.ToString(), tagContent);
        }

        public virtual void AddTag(string tagType, string tagContent)
        {
            var tag = new LogEntryTag();
            tag.ParentEntry = this;
            tag.TagType = tagType;
            tag.TagText = tagContent;
            EntryTags.Add(tag);
        }

        public override string ToString()
        {
            return $"LogEntry:{EntryTypeId} ({EntryTags.Select(t => $"{t.TagType}:{t.TagText}").SmartToString(";")})";
        }

        #endregion
    }


    public enum LogEntryTagType
    {
        FileName,
        Exception,
        XmlError,
        Position,
        Line,
        Module,
        Version
    }
}