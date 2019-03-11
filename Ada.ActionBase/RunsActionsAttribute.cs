namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RunsActionsAttribute : Attribute
    {
        #region  Constructors

        public RunsActionsAttribute(params Type[] actions)
        {
            if (actions.Any(c => !typeof(IAdaAction).IsAssignableFrom(c)))
                throw new ArgumentException(nameof(actions), "Most be of array type AdaActionBase");
            Actions = actions;
            ActionStrings = Enumerable.Empty<string>();
        }

        public RunsActionsAttribute(params string[] actions)
        {
//            if (actions.Any(c => !typeof(IAdaAction).IsAssignableFrom(c)))
//                throw new ArgumentException(nameof(actions), "Most be of array type AdaActionBase");
            ActionStrings = actions;
            Actions = Enumerable.Empty<Type>();
        }

        #endregion

        #region Properties

        public IEnumerable<Type> Actions { get; }
        public IEnumerable<string> ActionStrings { get; }

        #endregion
    }
}