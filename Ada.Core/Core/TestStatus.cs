namespace Ada.Core
{
    #region Namespace Using

    using System;
    using System.Drawing;
    using Log;
    using Properties;
    using SA;
//    using Ada.SA.Remoting;

    #endregion

    public class TestStatus
    {
        #region  Fields

        protected Image busyImage;

        private readonly Bitmap emptyBusyImage = new Bitmap(16, 16);

        protected string progress;

        protected Image statusImage;

        #endregion

        #region  Constructors

        public TestStatus()
        {
            Reset();
        }

        #endregion

        #region Properties

        public Image BusyImage
        {
            get => busyImage;
            set
            {
                if (busyImage == value)
                    return;

                busyImage = value;
                OnBusyImageChanged(EventArgs.Empty);
            }
        }

        public int ErrorCount { get; set; }


        public string Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnProgressChanged(EventArgs.Empty);
            }
        }

        public Image StatusImage
        {
            get => statusImage;
            set
            {
                if (statusImage == value)
                    return;

                statusImage = value;
                OnStatusImageChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region

        public event EventHandler BusyImageChanged;

        protected void OnBusyImageChanged(EventArgs e)
        {
            if (BusyImageChanged != null)
            {
                if (SafeInvoker.InvokeRequired())
                    SafeInvoker.SafeInvoke(BusyImageChanged, this, e);
                else
                    BusyImageChanged(this, e);
            }
        }

        protected void OnProgressChanged(EventArgs e)
        {
            if (ProgressChanged != null)
            {
                if (SafeInvoker.InvokeRequired())
                    SafeInvoker.SafeInvoke(ProgressChanged, this, e);
                else
                    ProgressChanged(this, e);
            }
        }

        protected void OnStatusImageChanged(EventArgs e)
        {
            if (StatusImageChanged != null)
            {
                if (SafeInvoker.InvokeRequired())
                    SafeInvoker.SafeInvoke(StatusImageChanged, this, e);
                else
                    StatusImageChanged(this, e);
            }
        }

        public void ProcessLoggingEvent(AdaTestLog_EventHandlerArgs e)
        {
            if (e.Event.Severity == 1)
                SetImage(LoggingEventType.Error);
            if (e.Event.Severity == 3)
                SetImage(LoggingEventType.Unknown);
        }

        public void ProcessLoggingEvent(LoggingEventArgs e)
        {
            if (e.EventType == LoggingEventType.ProgressUpdate) Progress = e.EventMessage;

            SetImage(e.EventType);
        }

        public event EventHandler ProgressChanged;

        public void Reset()
        {
            StatusImage = Resources.led_gray;
            BusyImage = emptyBusyImage;
            ErrorCount = 0;
            Progress = "0 %";
        }

        private void SetImage(LoggingEventType e)
        {
            switch (e)
            {
                case LoggingEventType.Unknown:
                    StatusImage = Resources.led_yellow;
                    break;
                case LoggingEventType.TestStart:
                    BusyImage = Resources.small_busy16x16;
                    StatusImage = Resources.led_green;
                    break;
                case LoggingEventType.TestEnd:
                    BusyImage = emptyBusyImage;
                    Progress = "100 %";
                    break;
                case LoggingEventType.FastRun:
                    BusyImage = emptyBusyImage;
                    break;
                case LoggingEventType.Error:
                    StatusImage = Resources.led_red;
                    ErrorCount++;
                    break;
                case LoggingEventType.TestSkipped:
                    if (ErrorCount == 0)
                        StatusImage = Resources.led_green;
                    BusyImage = emptyBusyImage;
                    break;
                case LoggingEventType.TestSkippedPreConditionsNotMet:
                    StatusImage = Resources.led_green;
                    BusyImage = emptyBusyImage;
                    break;
                case LoggingEventType.Info:
                    break;
            }
        }


        public event EventHandler StatusImageChanged;

        #endregion
    }
}