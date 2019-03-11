namespace Ada.Test.UnitTests.Actions
{
    #region Namespace Using

    using System.Linq;
    using System.Reflection;
    using Action64.IngestActions;
    using ActionBase;
    using Ada.Actions;
    using NUnit.Framework;

    #endregion

    [TestFixture(Category = "RequiredChecks")]
    public class RequiredChecksFoundInTop
    {
//        [Ignore("Turns out fixing this does would not fix the whole issue.")]
        [TestCase]
        public void HopeToBeTrue()
        {
            AdaActionContainer.Instance.Clear();
            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>(false));
            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<AdaSingleQueryAction>(false));

            var asses = new[]
                {typeof(DocumentsOtherIngestAction).Assembly, typeof(AdaSingleQueryAction).Assembly};

            foreach (var action in asses.SelectMany(
                a => a.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IAdaAction)))))
            {
                var topActions = AdaActionContainer.Instance.GetTopActions(action).ToList();

                foreach (var check in action.GetCustomAttributes<RequiredChecksAttribute>().SelectMany(a => a.Checks))
                foreach (var topAction in topActions)
                    Assert.IsTrue(topAction.GetCustomAttributes<RequiredChecksAttribute>().Any(att => att.Checks.Contains(check)),
                        $"{action.FullName}'s required check {check.FullName} is not found in the topaction {topAction.FullName} .");
            }
        }
    }
}