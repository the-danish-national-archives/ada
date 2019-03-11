namespace Ada.Core
{
    #region Namespace Using

    using System;
    using ActionBase;
    using Ra.Common.Reflection;

    #endregion

    public struct ExecutionOrder
    {
        public Type Action { get; }

        public string AvStatusName { get; }

        public string VisualName { get; }


        public ExecutionOrder(Type action, string avStatusName, string visualName)
        {
            if (!typeof(IAdaAction).IsAssignableFrom(action))
                throw new ArgumentException(nameof(action), "Type should be assignable from IAdaAction");
            Action = action;
            AvStatusName = avStatusName;
            VisualName = visualName;
        }

        public ExecutionOrder(Type action, string avStatusName, Type typeForProperty)
            : this(action, avStatusName, UILabelsAttribute.GetUIName(avStatusName, typeForProperty))
        {
        }
    }
}