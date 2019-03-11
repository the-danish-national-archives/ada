#region Header

// Author 
// Created 16

#endregion

namespace Ada.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using Core;
    using GalaSoft.MvvmLight;
    using Log;
    using Log.Entities;
    using Ra.Common.Algorithms;
    using Ra.Common.ExtensionMethods;

    #endregion

    public class CheckStatusViewModel : ViewModelBase
    {
        #region  Fields

        private bool? isRunning = false;


        private CheckTreeItemStatus? status = CheckTreeItemStatus.ToBeDone;

        #endregion

        #region  Constructors

        public CheckStatusViewModel(CheckTreeItem owner)
        {
            Owner = owner;


            Status = CheckTreeItemStatus.Unknown;

            owner.PropertyChanged += Owner_PropertyChanged;

            Controller.LoggingEvent += Controller_LoggingEvent;
            owner.SubItems.CollectionChanged += SubItems_CollectionChanged;
        }

        #endregion

        #region Properties

        public bool IsRunning
        {
            get
            {
                return isRunning ?? (isRunning = Owner.SubItems.Select(i => i.Status.IsRunning).Aggregate(
                           false,
                           (finalStatus, s) => finalStatus | s)).Value;
            }

            set => Set(ref isRunning, value);
        }

        private CheckTreeItem Owner { get; }

        public CheckTreeItemStatus Status
        {
            get
            {
                return status ?? (status = Owner.SubItems.Select(i => i.Status.Status).Aggregate(
                           CheckTreeItemStatus.Unknown,
                           (finalStatus, s) => s < finalStatus ? s : finalStatus)).Value;
            }

            set => Set(ref status, value);
        }

        #endregion

        #region

        private void AdaTestLog_Callback(LogEntry callback)
        {
            if (Owner.RelatedAction != null
                && Owner.RelatedAction.FullName != Algo.FindTop(callback.OwnerProcess, p => p.Parent).Type)
                return;
            // only run once
            AdaTestLog.UnsubscripeFromCheckName(Owner.CheckType?.FullName, AdaTestLog_Callback);

            Status = Owner.IsInfo ? CheckTreeItemStatus.Info : CheckTreeItemStatus.Error;
        }


        private void Controller_LoggingEvent(object sender, LoggingEventArgs loggingEvent)
        {
            if (loggingEvent.Source != Owner.RelatedAction)
                return;

            if (loggingEvent.EventType == LoggingEventType.TestStart)
                IsRunning = true;
            if (loggingEvent.EventType.IsStoppingEvent())
                IsRunning = false;

            if (!(Status == CheckTreeItemStatus.ToBeDone || Status == CheckTreeItemStatus.Running))
                return;

            switch (loggingEvent.EventType)
            {
                case LoggingEventType.TestStart:
                    Status = CheckTreeItemStatus.Running;
                    break;
                case LoggingEventType.Error:
                    Status = CheckTreeItemStatus.Error;
                    break;
                case LoggingEventType.Info:
                    Status = CheckTreeItemStatus.Warning;
                    break;
                case LoggingEventType.TestSkippedPreConditionsNotMet:
                    Status = CheckTreeItemStatus.Skipped;
                    break;
                case LoggingEventType.TestSkipped:
                    Status = CheckTreeItemStatus.ToBeDone;
                    break;
                case LoggingEventType.FastRun:
                case LoggingEventType.TestEnd:
                    Status = CheckTreeItemStatus.Finished;
                    break;
            }
        }

        private void Owner_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Owner.CheckType))
            {
                if (Owner.CheckType == null)
                    return;

                AdaTestLog.SubscripeToCheckName(Owner.CheckType.FullName, AdaTestLog_Callback);

                Status = CheckTreeItemStatus.ToBeDone;
            }
        }


        private void SubItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateStatus();
        }


        private void SubItems_CollectionChanged
            (object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems.AsEnumerableOrEmpty())
            {
                var checkTreeItem = item as CheckTreeItem;
                if (checkTreeItem == null)
                    continue;
                checkTreeItem.Status.PropertyChanged += SubItem_PropertyChanged;
            }

            foreach (var item in e.OldItems.AsEnumerableOrEmpty())
            {
                var checkTreeItem = item as CheckTreeItem;
                if (checkTreeItem != null)
                    checkTreeItem.Status.PropertyChanged -= SubItem_PropertyChanged;
            }

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            status = null;
            isRunning = null;
            RaisePropertyChanged(nameof(Status));
            RaisePropertyChanged(nameof(IsRunning));
        }

        #endregion
    }
}