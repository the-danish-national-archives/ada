namespace Ada.Core
{
    #region Namespace Using

    using System;

    #endregion

    public class LoggingEventArgs : EventArgs
    {
        #region  Constructors

        public LoggingEventArgs(Type source, LoggingEventType eventType, string eventMessage)
        {
            EventType = eventType;
            EventMessage = eventMessage;
            Source = source;
        }

        #endregion

        #region Properties

        public string EventMessage { get; }

        public LoggingEventType EventType { get; }

        public Type Source { get; }

        #endregion
    }

    public delegate void LoggingEventHandler(object sender, LoggingEventArgs loggingEvent);

    //
    public enum LoggingEventType
    {
        TestStart,
        ProgressUpdate,
        Error,
        Info,
        FastRun,
        TestEnd,
        TestSkipped,
        TestSkippedPreConditionsNotMet,
        Unknown
    }


    public static class LoggingEventTypeExtensions
    {
        #region

        public static bool IsStoppingEvent(this LoggingEventType eventType)
        {
            return eventType == LoggingEventType.TestEnd || eventType == LoggingEventType.TestSkipped
                                                         || eventType == LoggingEventType.TestSkippedPreConditionsNotMet || eventType == LoggingEventType.FastRun;
        }

        #endregion
    }
}