namespace Ada.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ChecksBase;
    using GalaSoft.MvvmLight;
    using JetBrains.Annotations;
    using Log.Entities;
    using Ra.Common.Wpf.ResultsList;
    using Text.Properties;

    #endregion

    public class LogEntryOpenWithViewModel : ViewModelBase, IResultsList
    {
        #region LogEntryOpenWithViewModelTypeEnum enum

        public enum LogEntryOpenWithViewModelTypeEnum
        {
            AvQuery,
            TestQuery,
            List
        }

        #endregion

        #region  Constructors

        public LogEntryOpenWithViewModel
            (LogEntryOpenWithViewModelTypeEnum type, object value, string text, string message)
        {
            LogEntryOpenWithViewModelType = type;
            Value = value;
            Text = text;
            Message = message;
        }

        #endregion

        #region Properties

        public LogEntryOpenWithViewModelTypeEnum LogEntryOpenWithViewModelType { get; }

        #endregion

        #region IResultsList Members

        public string Message { get; }

        public object Value { get; }

        public string Text { get; }

        #endregion

        #region

        public static IEnumerable<LogEntryOpenWithViewModel> GenerateList
            ([NotNull] Type checkType, [NotNull] LogEntry logEntry, string message = null)
        {
            return LogEntryToAvQuery(checkType, logEntry, message)
                .Union(LogEntryToTestQuery(checkType, logEntry, message))
                .Union(LogEntryToList(checkType, logEntry, message));
        }


        public static IEnumerable<LogEntryOpenWithViewModel> LogEntryToAvQuery
            ([NotNull] Type checkType, [NotNull] LogEntry logEntry, string message)
        {
            foreach (var source in checkType.GetMethods()
                .Select(m => new {m, att = m.GetCustomAttributes<AdaAvCheckToAvQueryAttribute>()})
                .Where(m => m.att != null && m.att.Any()))
            {
                var res = source.att.FirstOrDefault()?.CreateFromLogEntryToAvQueryConverter(source.m, logEntry);
                yield return
                    new LogEntryOpenWithViewModel(LogEntryOpenWithViewModelTypeEnum.AvQuery, res, UIText.RunAvQuery,
                        message);
            }
        }

        public static IEnumerable<LogEntryOpenWithViewModel> LogEntryToList
            ([NotNull] Type checkType, [NotNull] LogEntry logEntry, string message)
        {
            foreach (var attr in checkType.GetCustomAttributes<AdaAvCheckToResultsListAttribute>())
            {
                var res = attr.GetListGenerator(logEntry);
                yield return
                    new LogEntryOpenWithViewModel(LogEntryOpenWithViewModelTypeEnum.List, res, UIText.ListResults,
                        message);
            }
        }

        public static IEnumerable<LogEntryOpenWithViewModel> LogEntryToTestQuery
            ([NotNull] Type checkType, [NotNull] LogEntry logEntry, string message)
        {
            foreach (var source in checkType.GetMethods()
                .Select(m => new {m, att = m.GetCustomAttributes<AdaAvCheckToTestQueryAttribute>()})
                .Where(m => m.att != null && m.att.Any()))
            {
                var res = source.att.FirstOrDefault()?.GetTestQuery(source.m);
                yield return
                    new LogEntryOpenWithViewModel(LogEntryOpenWithViewModelTypeEnum.TestQuery, res, UIText.RunTestQuery,
                        message);
            }
        }

        #endregion
    }
}