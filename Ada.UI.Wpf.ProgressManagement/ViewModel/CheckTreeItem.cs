namespace Ada.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Markup;
    using ActionBase;
    using GalaSoft.MvvmLight;
    using JetBrains.Annotations;
    using Log;
    using Log.Entities;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Interfaces;
    using Ra.Common.Reflection;

    #endregion

    [ContentProperty("SubItems")]
    public class CheckTreeItem : ViewModelBase, IContainsChild
    {
        #region  Fields

        private readonly CheckTreeItem parent;

        private string _check;

        private Type _checkType;

        private string _desc;

        private bool _isError = true;

        private bool? _toBeRun;
        private bool _toBeRunIsSignivigant;

        private bool _toBeRunNeedsUpdating;

        private Type relatedAction;

        private AdaTestLog testLog;

        #endregion

        #region  Constructors

        public CheckTreeItem()
        {
            Notifications = new NotificationsListViewModel(this);
            Status = new CheckStatusViewModel(this);

            SubItems.CollectionChanged += SubItems_CollectionChanged;
        }


        private CheckTreeItem(CheckTreeItem parent, Type action) : this()
        {
            this.parent = parent;
//            Desc = parent.Desc;
            ShortDesc = UILabelsAttribute.GetUIName(action) ?? action.Name;
//            InternalTestName = parent.InternalTestName;
            ToBeRun = parent.ToBeRun;


            // Do not trigger auto set, by setting in correct order
            RelatedAction = action;
            CheckType = parent.CheckType;
            Check = parent.Check;
        }

        #endregion

        #region Properties

        public string Check
        {
            get => _check;

            set
            {
                // Only allow check to be set once, so related properties are also only set once
                if (_check != null)
                    throw new InvalidOperationException();
                if (Set(ref _check, value))
                    UpdateCheckType();
            }
        }

        [CanBeNull]
        public Type CheckType
        {
            get => _checkType;

            private set
            {
                if (Set(ref _checkType, value))
                {
                    if (RelatedAction != null)
                        return;
                    var actions = GetRelatedActions(_checkType).AsEnumerableOrEmpty().ToList();
                    if (actions.Count > 1)
                        foreach (var action in actions)
                        {
                            var subItem = new CheckTreeItem(this, action);
                            SubItems.Add(subItem);
                        }
                    else
                        RelatedAction = actions.FirstOrDefault();
                }
            }
        }

        public string Desc
        {
            get => _desc ?? parent?.Desc;
            set => _desc = value;
        }

        public string FormattedText { get; set; }

        public string InternalTestName { get; set; }

        public bool IsError
        {
            get => parent?.IsError ?? _isError;
            set => _isError = value;
        }

        public bool IsInfo => !IsError;

        public NotificationsListViewModel Notifications { get; set; }

        public bool PreExpanded { get; set; } = false;

        [CanBeNull]
        public Type RelatedAction
        {
            get => relatedAction;

            private set
            {
                if (Set(ref relatedAction, value)) ToBeRun = true;
            }
        }

        public ObservableCollection<CheckTreeItem> Requirements { get; } = new ObservableCollection<CheckTreeItem>();

        public string ShortDesc { get; set; }

        public CheckStatusViewModel Status { get; set; }

        public ObservableCollection<CheckTreeItem> SubItems { get; } = new ObservableCollection<CheckTreeItem>();

        public AdaTestLog TestLog
        {
            get => testLog;

            set
            {
                if (Set(ref testLog, value))
                    foreach (var item in SubItems)
                        item.TestLog = testLog;
            }
        }

        public bool? ToBeRun
        {
            get
            {
                if (_toBeRunNeedsUpdating)
                {
                    var haveTrueAndFalse = SubItems.AggregateWhile(
                        new {t = false, f = false}, // collect if-seen
                        (b, i) =>
                        {
                            var sub = i.ToBeRun;

                            // If null on composite check both true and false have been seen inside it
                            if (sub == null && i._toBeRunIsSignivigant)
                                return new {t = true, f = true};

                            return new
                            {
                                t = b.t | (sub == true),
                                f = b.f | (sub == false)
                            };
                        }
                        ,
                        b => !(b.t && b.f)
                    );

//                    var temp =
//                        haveTrueAndFalse.t
//                        ? (haveTrueAndFalse.f ? (bool?)null : true)
//                        : (haveTrueAndFalse.f ? (bool?)false : null);
                    var temp =
                        haveTrueAndFalse.t == haveTrueAndFalse.f
                            ? null
                            : (bool?) haveTrueAndFalse.t;

                    _toBeRunIsSignivigant = haveTrueAndFalse.t || haveTrueAndFalse.f;

                    _toBeRunNeedsUpdating = false;
                    if (_toBeRun != temp)
                        Set(ref _toBeRun, temp);
                }

                return _toBeRun;
            }
            set
            {
                if (value == null)
                    value = false;
                if (SubItems.Any())
                {
                    _toBeRunNeedsUpdating = true;
                    foreach (var item in SubItems) item.ToBeRun = (bool) value;
                    RaisePropertyChanged(nameof(ToBeRun));
                }
                else
                {
                    if (RelatedAction != null)
                        Set(ref _toBeRun, value);
                }
            }
        }

        #endregion

        #region IContainsChild Members

        public bool HasChild(object child)
        {
            return GetAllCheckTreeItems().Contains(child);
        }

        #endregion

        #region

        public IEnumerable<CheckTreeItem> GetAllCheckTreeItems()
        {
            return
                SubItems.AsEnumerableOrEmpty().SelectMany(i => i.GetAllCheckTreeItems()).Union(this.YieldOrEmpty());
        }

        public IEnumerable<LogEntryOpenWithViewModel> GetLogEntryOpenWithList(LogEntry logEntry)
        {
            if (CheckType == null)
                return null;
            var res = LogEntryOpenWithViewModel.GenerateList(CheckType, logEntry, Desc).ToList();
            return res.Any() ? res : null;
        }

        [CanBeNull]
        private static IEnumerable<Type> GetRelatedActions(Type checkType)
        {
            return checkType == null
                ? null
                : AdaActionContainer.Instance.GetTopActions(AdaActionContainer.Instance.GetActions(checkType));
        }

        private void SubItemOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateToBeRun();
        }

        private void SubItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems.AsEnumerableOrEmpty())
            {
                var checkTreeItem = item as CheckTreeItem;
                if (checkTreeItem == null)
                    continue;
                checkTreeItem.TestLog = TestLog;

                checkTreeItem.PropertyChanged += SubItemOnPropertyChanged;

                UpdateToBeRun();
            }
        }

        private void UpdateCheckType()
        {
            if (CheckType != null)
                return;

            CheckType = AdaActionContainer.Instance.GetCheckType(Check);
        }

        private void UpdateToBeRun()
        {
//            if (SubItems.Any())
            if (_toBeRunNeedsUpdating)
                return;
            _toBeRunNeedsUpdating = true;
            RaisePropertyChanged(nameof(ToBeRun));
        }

        #endregion
    }
}