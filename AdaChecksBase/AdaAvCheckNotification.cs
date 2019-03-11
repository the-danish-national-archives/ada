namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Log.Entities;

    #endregion

    public class AdaAvCheckNotification
    {
        #region  Constructors

        protected AdaAvCheckNotification(string tagType)
        {
            TagType = tagType;
        }

        #endregion

        #region Properties

        private string TagType { get; }

        #endregion

        #region

        protected virtual IEnumerable<Tuple<string, string>> GetEntryTags()
        {
            foreach (var property in GetType().GetProperties())
            {
                var attr = property.GetCustomAttribute<AdaAvCheckNotificationTagAttribute>();
                if (attr == null)
                    continue;

                var name = attr.Name;
                if (string.IsNullOrWhiteSpace(name))
                    name = property.Name;

                yield return new Tuple<string, string>(name, attr.ValueToString(property.GetValue(this)));
            }
        }


        private string GetEntryTypeId()
        {
            return TagType;
        }

        public LogEntry ToLogEntry()
        {
            var res = new LogEntry
            {
                EntryTypeId = GetEntryTypeId()
            };
            foreach (var tag in GetEntryTags()) res.AddTag(tag.Item1, tag.Item2);
            res.CheckName = GetType().FullName;
            return res;
        }

        #endregion
    }
}