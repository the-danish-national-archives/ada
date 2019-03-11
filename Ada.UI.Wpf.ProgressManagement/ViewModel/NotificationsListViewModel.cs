namespace Ada.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using ChecksBase;
    using JetBrains.Annotations;
    using Log;
    using Ra.Common.Algorithms;
    using Ra.Common.Wpf.Utils;
    using Text.Properties;

    #endregion

    public class NotificationsListViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Static

        protected const int MaxNotifications = 10000;

        public const string OwnerColumnText = "Owner";
        public const string OpenWithListColumnText = "OpenWithList";
        public const string LogEntryColumnText = "LogEntry";
        public const string IsImageProperty = "IsImage";

        #endregion

        #region  Fields

        private readonly CheckTreeItem owner;

        #endregion

        #region  Constructors

        public NotificationsListViewModel(CheckTreeItem owner)
        {
            this.owner = owner;
            AdaTestLog.LoggingEvent += AdaTestLogOnLoggingEvent;
        }

        #endregion

        #region Properties

        public DataTable List
        {
            get
            {
                var table = new DataTable();
                var logEntryColumn = new DataColumn(LogEntryColumnText);
                logEntryColumn.DataType = typeof(object);
                logEntryColumn.ExtendedProperties[HideColumns.ExtendedPropertyForHideName] = true;
                table.Columns.Add(logEntryColumn);
                var ownerColumn = new DataColumn(OwnerColumnText);
                ownerColumn.DataType = typeof(object);
                ownerColumn.ExtendedProperties[HideColumns.ExtendedPropertyForHideName] = true;
                table.Columns.Add(ownerColumn);
                var openWithListColumn = new DataColumn(OpenWithListColumnText);
                openWithListColumn.DataType = typeof(object);
                openWithListColumn.ExtendedProperties[HideColumns.ExtendedPropertyForHideName] = true;
                table.Columns.Add(openWithListColumn);
                var rightIconColumn = new DataColumn(" ");
                rightIconColumn.DataType = typeof(object);
                rightIconColumn.ExtendedProperties[IsImageProperty] = true;
                table.Columns.Add(rightIconColumn);
                var severityColumn = new DataColumn(UIText.SeverityType);
                table.Columns.Add(severityColumn);
                var descColumn = new DataColumn(UIText.Description);
                table.Columns.Add(descColumn);

                if (owner.TestLog == null)
                    return table;

                var checkName = owner.GetAllCheckTreeItems().Where(i => i.CheckType != null)
                    .ToDictionary(i => new Tuple<string, string>(i.CheckType.FullName, i.RelatedAction?.FullName));


                foreach (var entry in
                    owner.TestLog.GetEntries(
                        checkName.ToLookup(p => p.Key.Item1, p => p.Key.Item2)
                        , MaxNotifications
                    ))
                {
                    var entryOwner = checkName[new Tuple<string, string>(entry.CheckName, Algo.FindTop(entry.OwnerProcess, p => p.Parent).Type)];
                    var row = table.NewRow();


                    row[logEntryColumn] = entry;
                    row[ownerColumn] = entryOwner;
                    var menu = row[openWithListColumn] = entryOwner.GetLogEntryOpenWithList(entry);
                    row[rightIconColumn] = menu != null;
                    row[severityColumn] = entryOwner.IsError ? UIText.SeverityTypeError
                        : entryOwner.IsInfo ? UIText.SeverityTypeInfo
                        : "";

                    row[UIText.Description] = entry.FormattedText;
                    descColumn.ExtendedProperties["TimesUsed"] = (descColumn.ExtendedProperties["TimesUsed"] as int? ?? 0) + 1;

                    foreach (var tag in entry.EntryTags)
                    {
                        if (
                            entryOwner.CheckType?.GetProperty(tag.TagType)
                                ?.GetCustomAttribute<AdaAvCheckNotificationTagAttribute>(true)
                                ?.Hidden ?? false)
                            continue;
                        var column = table.Columns[tag.TagType];

                        if (column == null)
                        {
                            column = new DataColumn(tag.TagType);
                            table.Columns.Add(column);
                        }

                        column.ExtendedProperties["TimesUsed"] = (column.ExtendedProperties["TimesUsed"] as int? ?? 0) + 1;

                        row[tag.TagType] = tag.TagText;
                    }

                    table.Rows.Add(row);
                }

                var totalRows = table.Rows.Count;

                var columnsToRemove = table.Columns.Cast<DataColumn>()
                    .Where(c => (c.ExtendedProperties["TimesUsed"] as int? ?? 0) / (double) totalRows < 0.9).ToList();
                columnsToRemove.Remove(descColumn);
                columnsToRemove.Remove(ownerColumn);
                columnsToRemove.Remove(openWithListColumn);
                columnsToRemove.Remove(rightIconColumn);
                columnsToRemove.Remove(severityColumn);
                columnsToRemove.Remove(logEntryColumn);
                foreach (var column in columnsToRemove) table.Columns.Remove(column);


                if (totalRows >= MaxNotifications)
                {
                    var row = table.NewRow();
                    row[UIText.Description] = UIText.MaxRowsReached;
                    row[OwnerColumnText] = this;
                    table.Rows.Add(row);
                }

                return table;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            AdaTestLog.LoggingEvent -= AdaTestLogOnLoggingEvent;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        private void AdaTestLogOnLoggingEvent(object sender, AdaTestLog_EventHandlerArgs eventArgs)
        {
            OnPropertyChanged(nameof(List));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}