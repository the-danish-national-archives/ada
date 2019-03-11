namespace Ra.DocumentInvestigator.AdaAvChecking.AdaReporting
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class DocLogEntryFromDocInfo : DocLogEntry
    {
        #region  Constructors

        protected DocLogEntryFromDocInfo(DocInfo docInfo, string eventTypeId)
        {
            EventTags = new List<DocLogEntryTag>();
            TimeStamp = $"{DateTime.Now:O}";

            EventTypeId = eventTypeId;
            AddTag("Path", docInfo.DocumentFolder);
            AddTag("DocumentID", docInfo.DocumentId);
        }

        #endregion
    }
}