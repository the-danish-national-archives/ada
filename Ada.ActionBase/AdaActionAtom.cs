namespace Ada.ActionBase
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ChecksBase;
    using Common;
    using Log;
    using Log.Entities;
    using Repositories;

    #endregion

    public abstract class AdaActionAtom<TSubject>
    {
        #region  Fields

        private readonly IAdaProcessLog processLog;

        private readonly IAdaRepository targetRepository;

        protected readonly AdaTestLog testLog; //Use Interface again

        private ProcessLogEntry processLogEntry;

        #endregion

        #region  Constructors

        protected AdaActionAtom(IAdaProcessLog processLog, IAdaTestLog testLog, AVMapping mapping)
        {
            this.processLog = processLog;
            this.testLog = testLog as AdaTestLog; //bah!
            Mapping = mapping;
        }

        #endregion

        #region Properties

        public AVMapping Mapping { get; }

        #endregion

        #region

        protected bool CanRun()
        {
            var ids = GetRequirementsFromAttributes().Union(GetOtherRequirements());


            foreach (var violation in ids)
                //                if (violation.Target != null)
//                {
//                    var ownerProcess = processLog.GetExistingProcessLog(
//                        violation.Target);
//
//
//                    // TODO: 22/11/2016 include again, and other checks?
//                    if (ownerProcess == null)
//                        continue;
//                    //                    return false;
//                }


            foreach (var check in violation.Precludes)
            {
                var checkName = check.Name;
                var ownerFound = false;
                var ownerFoundNeeded = false;

                foreach (var entry in testLog.GetEntries(check.FullName))
                {
                    ownerFoundNeeded = true;
                    var owner = entry.OwnerProcess;
                    while (owner != null)
                    {
                        if (owner.Type == violation.Type)
                            if (violation.Target == null || owner.InternalName == violation.Target)
                                ownerFound = true;


                        owner = owner.Parent;
                    }
                }

                if (ownerFoundNeeded && ownerFound)
                    return false;
            }

//                var associatedEntries = testLog.GetAssociatedEntries(
//                    ownerProcess);
//
//                if (associatedEntries.Any(e => violation.Precludes.Select(t => t.FullName).Contains(e.CheckName)))
//                    return false;


            return true;
        }


        protected virtual void Clear(ProcessLogEntry previousProcessLogEntry)
        {
            testLog.DeleteAlongWithAssociatedEntries(previousProcessLogEntry);
        }

        protected void CompleteProcessLogging()
        {
            if (processLogEntry != null)
                processLog.ProcessCompleted(processLogEntry);
            testLog.Flush();
        }

        protected IAdaTestLog GetAdaTestLog()
        {
            return testLog;
        }


        public string GetId(TSubject testSubject)
        {
            return GetType() + ">" + typeof(TSubject) + ">" + testSubject;
        }

        public static string GetId<T>(TSubject testSubject)
        {
            return typeof(T) + ">" + typeof(TSubject) + ">" + testSubject;
        }

        public static string GetId(Type action, object testSubject)
        {
            return action + ">" + (testSubject is Type ? "System.Type" : testSubject.GetType().ToString()) + ">" +
                   testSubject;
        }

        protected virtual IEnumerable<AdaActionRequirement> GetOtherRequirements()
        {
            return Enumerable.Empty<AdaActionRequirement>();
        }

        protected IEnumerable<AdaActionRequirement> GetRequirementsFromAttributes()
        {
            return GetType().GetTypeInfo().GetCustomAttributes<RequiredChecksAttribute>(false)
                .SelectMany(c => AdaActionRequirement.MultiFromChecks(c.Checks));
        }

        protected IAdaProcessLog GetSubordinateProcessLog()
        {
            return processLog.GetSubOrdinateProcessLog(processLogEntry);
        }

        protected void HandleException(Exception e)
        {
            testLog.Flush();
            if (e is AdaSkipActionException || e.InnerException is AdaSkipActionException)
                return;

            if (e is AdaSkipAllActionException || e.InnerException is AdaSkipAllActionException)
                throw e;

            Report(new AdaAvInternalError(e, GetType()));

            CompleteProcessLogging();
        }


        // regex
        // from
        /*
            * var logEntry = new LogEntry { EntryTypeId = "([^"]+)"([^"]*"([^"]+)", ([^;]*);)?([^"]*"([^"]+)", ([^;]*);)?([^"]*"([^"]+)", ([^;]*);)?([^"]*"([^"]+)", ([^;]*);)?([^"]*"([^"]+)", ([^;]*);)?([^"]*"([^"]+)", ([^;]*);)?[^;]*;
        */
        // to
        /*
            \t: base("$1")\n\t{\n\t\t$3 = ($4;\n\t\t$6 = ($7;\n\t\t$9 = ($10;\n\t\t$12 = ($13;\n\t\t$15 = ($16;\n\t\t$18 = ($19;
        */
        protected void Report(AdaAvCheckNotification notification)
        {
            ReportLogEntry(notification.ToLogEntry());
        }

        protected bool ReportAny(IEnumerable<AdaAvCheckNotification> notifications)
        {
            var any = false;
            foreach (var notification in notifications)
            {
                Report(notification);
                any = true;
            }

            return any;
        }


        protected void ReportLogEntry(LogEntry logEntry)
        {
            logEntry.OwnerProcess = processLogEntry;
            testLog.LogError(logEntry);
        }


        protected virtual bool Skippable(TSubject testSubject)
        {
            var previousProcessLogEntry = processLog.GetExistingProcessLog(GetId(testSubject));

            if (previousProcessLogEntry == null)
                return false;

            if (previousProcessLogEntry.StopTime == null)
            {
                Clear(previousProcessLogEntry);
                return false;
            }

            testLog.ReplayAssociatedEntries(previousProcessLogEntry);
            return true;
        }

        protected void StartProcessLogging(TSubject testSubject)
        {
            processLogEntry = processLog.ProcessStarted(GetType().FullName, GetId(testSubject));
        }

        #endregion
    }


    public class AdaActionRequirement
    {
        #region  Constructors

        public AdaActionRequirement(Type type, object target, IEnumerable<Type> precludes)
        {
            Type = type.FullName;
            Precludes = precludes;
            Target = target == null ? null : AdaActionAtom<object>.GetId(type, target);
        }

        public AdaActionRequirement(Type type, object target, params Type[] precludes)
            : this(type, target, (IEnumerable<Type>) precludes)
        {
        }

        #endregion

        #region Properties

        public IEnumerable<Type> Precludes { get; }

        public string Target { get; }

        public string Type { get; }

        #endregion

        #region

        public static AdaActionRequirement FromChecks(object target, params Type[] precludes)
        {
            // TODO support multiple actions from check?
            var action = AdaActionContainer.Instance.GetActions(precludes[0]).FirstOrDefault();
            return new AdaActionRequirement(action, target, precludes);
        }

        public static AdaActionRequirement FromChecks(params Type[] precludes)
        {
            // TODO support multiple actions from check?
            var action = AdaActionContainer.Instance.GetActions(precludes[0]).FirstOrDefault();
            return new AdaActionRequirement(action, null, precludes);
        }

        public static IEnumerable<AdaActionRequirement> MultiFromChecks(params Type[] precludes)
        {
            return MultiFromChecks(precludes);
        }

        public static IEnumerable<AdaActionRequirement> MultiFromChecks(IEnumerable<Type> precludes)
        {
            foreach (var group in precludes.SelectMany(pre =>
                    AdaActionContainer.Instance.GetActions(pre).Select(action => new {pre, action}))
                .GroupBy(g => g.action))
                yield return new AdaActionRequirement(group.Key, null, group.Select(g => g.pre));
        }

        #endregion
    }
}