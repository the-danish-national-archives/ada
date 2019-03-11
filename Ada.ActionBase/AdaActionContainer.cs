namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Ra.Common.ExtensionMethods;

    #endregion

    public class AdaActionContainer
    {
        #region  Fields

        private readonly Dictionary<string, Type> allActions = new Dictionary<string, Type>();
        private readonly Dictionary<Type, ICollection<Type>> callersOfActions = new Dictionary<Type, ICollection<Type>>();
        private readonly Dictionary<Type, ICollection<Type>> checkToActions = new Dictionary<Type, ICollection<Type>>();

        #endregion

        #region  Constructors

        private AdaActionContainer()
        {
        }

        #endregion

        #region Properties

        public static AdaActionContainer Instance { get; } = new AdaActionContainer();

        #endregion

        #region

        public void Clear()
        {
            checkToActions.Clear();
            allActions.Clear();
            callersOfActions.Clear();
        }

        public Type GetAction(string actionName)
        {
            return allActions.GetOrDefault(actionName);
        }

        public IEnumerable<Type> GetActions([NotNull] Type check)
        {
            return checkToActions[check];
        }

        public Type GetCheckType(string check)
        {
            return checkToActions.Keys.FirstOrDefault(k => k.Name == check);
        }

        public IEnumerable<Type> GetTopActions(Type action)
        {
            return GetTopActions(action.Yield());
        }

        public IEnumerable<Type> GetTopActions(IEnumerable<Type> actions)
        {
            // Simple A* but with multiple results?
            var res = new HashSet<Type>();
            var breakOut = callersOfActions.Count;
            breakOut *= breakOut;
            var toLookAt = new Queue<Type>();
            Type next;
            var seen = new HashSet<Type>();
            foreach (var action in actions)
            {
                toLookAt.Enqueue(action);
                seen.Add(action);
            }

            while (toLookAt.Count > 0)
            {
                next = toLookAt.Dequeue();
                var parents = callersOfActions.GetOrDefault(next);
                if (parents == null)
                    res.Add(next);
                else
                    foreach (var parent in parents)
                    {
                        if (seen.Contains(parent))
                            continue;
                        seen.Add(parent);
                        toLookAt.Enqueue(parent);
                    }
            }

            return res;
        }

        public void Load(Assembly ass, bool throwOnAlreadyExisting = true)
        {
            foreach (var action in ass.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IAdaAction))))
            {
                foreach (var check in action.GetCustomAttributes<ReportsChecksAttribute>().SelectMany(a => a.Checks))
                {
                    var entry = checkToActions.GetOrDefault(check)
                                ?? (checkToActions[check] = new LinkedList<Type>());

                    entry.Add(action);
                }

                foreach (var calledAction in action.GetCustomAttributes<RunsActionsAttribute>()
                    .SelectMany(a => a.Actions.Union(a.ActionStrings.Select(s => Instance.GetAction(s)))))
                {
                    var entry = callersOfActions.GetOrDefault(calledAction)
                                ?? (callersOfActions[calledAction] = new LinkedList<Type>());

                    entry.Add(action);
                }

                var name = action.Name;
                if (!throwOnAlreadyExisting)
                    if (allActions.ContainsKey(name))
                        continue;
                allActions.Add(name, action);
            }
        }

        public void Load<T>(bool throwOnAlreadyExisting = true)
        {
            Load(typeof(T).Assembly, throwOnAlreadyExisting);
        }

        #endregion
    }
}