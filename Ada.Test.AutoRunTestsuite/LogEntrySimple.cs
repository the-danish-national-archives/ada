namespace Ada.Test.AutoRunTestsuite
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using Log.Entities;
    using Properties;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities;
    using Repositories;

    #endregion

    [DataContract(Name = "LogEvent")]
    public class LogEntrySimple
    {
        #region  Constructors

        public LogEntrySimple()
        {
            EntryTypeId = null;
        }

        public LogEntrySimple(LogEntry logEntry)
        {
            EntryTypeId = logEntry.EntryTypeId;
            Tags = logEntry.EntryTags.Select(t => new LogEntrySimpleTag(t)).ToList();
            FormattedText = logEntry.FormattedText;
//            TimeStamp = logEntry.TimeStamp;
        }

        #endregion

        #region Properties

        [DataMember]
        public bool ChangeNoticed { get; set; }

        [DataMember(Name = "EventTypeId")]
        public string EntryTypeId { get; set; }

        [DataMember]
        public string FormattedText { get; set; }

        [DataMember]
        public IList<LogEntrySimpleTag> Tags { get; set; }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LogEntrySimple) obj);
        }

        private bool Equals(LogEntrySimple other)
        {
            if (EntryTypeId == "0.1" && string.Equals(EntryTypeId, other.EntryTypeId))
                return true;
            return string.Equals(EntryTypeId, other.EntryTypeId) && (other.Tags == null ? Tags == null : Tags?.SequenceEqual(other.Tags) ?? false);
        }


        public static List<LogEntrySimple> GetEventLogsFromDb(DirectoryInfo testDir, DirectoryInfo outputDirectory)
        {
            List<LogEntrySimple> events;

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            InitializationMethods.SetDbCreationDirectory(outputDirectory.FullName);

            //            using (IUnitOfWork unitOfWork = AdaUowFactory.Instance.GetTestDbUnitOfWork(new AViD(testDir.Name), outputDirectory.FullName))
            //            {
            using (var unitOfWork = new AdaLogUowFactory(new AViD(testDir.Name), "log", new DirectoryInfo(Settings.Default.DBCreationFolder)).GetUnitOfWork())
            {
                var repository = unitOfWork.GetRepository<LogEntry>();

                var query = repository.All();
                var logEntries = query.ToArray();


                events = logEntries.Select(l => new LogEntrySimple(l)).ToList();

//                events.Sort(Comparer<LogEntrySimple>.Create((a, b) => string.CompareOrdinal(a.TimeStamp, b.TimeStamp)));
            }

            return events;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((EntryTypeId?.GetHashCode() ?? 0) * 397) ^ (Tags?.GetHashCode() ?? 0);
            }
        }

        public static List<LogEntrySimple> ReadEntryLogFile(string path)
        {
            try
            {
                var fileStream = new FileStream(path, FileMode.Open);

                return ReadEntryLogStream(fileStream);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private static List<LogEntrySimple> ReadEntryLogStream(Stream fileStream)
        {
            List<LogEntrySimple> events;

            var serializer = new DataContractSerializer(
                typeof(List<LogEntrySimple>));

            try
            {
                using (var stream = fileStream)
                using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings()))
                {
                    events = (List<LogEntrySimple>) serializer.ReadObject(xmlReader);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return events;
        }

        public static List<LogEntrySimple> ReadEntryLogString(string xml)
        {
            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(xml ?? ""));
//            var fileStream = new StringReader(xml);
            return ReadEntryLogStream(fileStream);
        }

//        [DataMember]
//        public string TimeStamp { get; set; }

        public override string ToString()
        {
            return $"<<{EntryTypeId}: {Tags.Select(t => t.ToString()).SmartToString()}>>\n{FormattedText}";
        }


        public static void WriteEventLogFile(string path, List<LogEntrySimple> events)
        {
            var result = new StringBuilder();

            var fileInfo = new FileInfo(path);
            var directoryInfo = fileInfo.Directory;
            if (!directoryInfo.Exists)
                directoryInfo.Create();

            using (var stream = new FileStream(path, FileMode.Create))
            {
                WriteEventLogStream(events, stream);
            }
        }

        private static void WriteEventLogStream(List<LogEntrySimple> events, Stream stream)
        {
            var serializer = new DataContractSerializer(
                events.GetType());

            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8}))
            {
                serializer.WriteObject(xmlWriter, events);

                xmlWriter.Flush();
            }
        }

        public static string WriteEventLogString(List<LogEntrySimple> events)
        {
            using (var stream = new MemoryStream())
            {
                WriteEventLogStream(events, stream);
                return Encoding.UTF8.GetString(stream.ToArray(), 0, (int) stream.Length);
            }
        }

        #endregion
    }

    [DataContract(Name = "LogEventSimpleTag")]
    public class LogEntrySimpleTag
    {
        #region  Constructors

        public LogEntrySimpleTag()
        {
            Type = "";
            Text = "";
        }

        public LogEntrySimpleTag(LogEntryTag logEntryTag)
        {
            Type = logEntryTag.TagType;
            Text = logEntryTag.TagText;
        }

        #endregion

        #region Properties

        [DataMember(Order = 1)]
        public string Text { get; set; }

        [DataMember(Order = 0)]
        public string Type { get; set; }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LogEntrySimpleTag) obj);
        }

        protected bool Equals(LogEntrySimpleTag other)
        {
            // text from xml-files have \r, text from database might not
            return string.Equals(Type?.Replace("\r", "").Replace("\n", "").Replace("¤", "").Replace("\t", "")
                       , other.Type?.Replace("\r", "").Replace("\n", "").Replace("¤", "").Replace("\t", ""))
                   && string.Equals(Text?.Replace("\r", "").Replace("\n", "").Replace("¤", "").Replace("\t", ""),
                       other.Text?.Replace("\r", "").Replace("\n", "").Replace("¤", "").Replace("\t", ""));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type?.GetHashCode() ?? 0) * 397) ^ (Text?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return $@"{Type}={Text}";
        }

        #endregion
    }
}