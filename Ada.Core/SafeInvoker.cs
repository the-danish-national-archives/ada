namespace SA
{
    #region Namespace Using

    using System;
    using System.ComponentModel;

    #endregion

    // TODO the use of this class seems like a bad pattern, consider fixing the problems be using Invoke inside target
    public static class SafeInvoker
    {
        #region Static

        private static ISynchronizeInvoke _syncDummy;

        #endregion

        #region

        public static bool InvokeRequired()
        {
            if (_syncDummy != null)
                return _syncDummy.InvokeRequired;
            return false;
        }

        public static void SafeInvoke(PropertyChangedEventHandler ev, object sender, PropertyChangedEventArgs e)
        {
            foreach (var d in ev.GetInvocationList())
            {
                _syncDummy.Invoke(d, new[] {sender, e});
                var target = d.Target;
            }
        }

        public static void SafeInvoke(EventHandler ev, object sender, EventArgs e)
        {
            foreach (var d in ev.GetInvocationList())
            {
                _syncDummy.Invoke(d, new[] {sender, e});
                var target = d.Target;
            }
        }

        public static void SetSyncContext(ISynchronizeInvoke syncDummy)
        {
            _syncDummy = syncDummy;
        }

        #endregion
    }
}