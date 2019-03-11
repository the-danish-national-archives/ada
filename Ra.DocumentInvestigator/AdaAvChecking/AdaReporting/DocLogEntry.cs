namespace Ra.DocumentInvestigator.AdaAvChecking.AdaReporting
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class DocLogEntry
    {
        #region  Constructors

        public DocLogEntry()
        {
            EventTags = new List<DocLogEntryTag>();
            TimeStamp = $"{DateTime.Now:O}";
        }

        #endregion

        #region Properties

        public int EventId { get; set; }

//        public string FormattedText { get; set; }
        public IList<DocLogEntryTag> EventTags { get; set; }

//        public int Severity { get; set; }
        public string EventTypeId { get; set; }
        public string TimeStamp { get; set; }

        #endregion

        #region

//        public Guid SessionKey { get; set; }

//        public void AddTag(DocLogEntryTagType tagType, string tagContent)
//        {
//            AddTag(tagType.ToString(), tagContent);
//        }

        protected void AddTag(string tagType, string tagContent)
        {
            var tag = new DocLogEntryTag
            {
                ParentEntry = this,
                TagType = tagType,
                TagText = tagContent
            };
            EventTags.Add(tag);
        }

        #endregion
    }

//    public enum DocLogEntryTagType
//    {
//        //todo fill up
//    }

    public struct DocLogEntryTag
    {
        public DocLogEntry ParentEntry { get; set; }

//        public int TagId { get; set; }
        public string TagType { get; set; }
        public string TagText { get; set; }
    }
}