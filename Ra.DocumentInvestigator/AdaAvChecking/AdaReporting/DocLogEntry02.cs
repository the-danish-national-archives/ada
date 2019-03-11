namespace Ra.DocumentInvestigator.AdaAvChecking.AdaReporting
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    public class DocLogEntry02 : DocLogEntry
    {
        #region  Constructors

        public DocLogEntry02(string name, Exception e)
        {
            EventTags = new List<DocLogEntryTag>();
            TimeStamp = $"{DateTime.Now:O}";

            EventTypeId = "0.2";
            AddTag(
                "FileName", name);
            AddTag("Exception", $"TiffPageReadException:{e.Message}");
        }

        #endregion
    }
}