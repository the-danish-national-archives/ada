namespace Ada.Test.UnitTests.Actions
{
    #region Namespace Using

    using NUnit.Framework;

    #endregion

    [TestFixture(Category = "ActionsAccountedFor")]
    public class ActionsAccountedFor
    {
//        [TestCase]
//        public void NotTooMany()
//        {
//            AdaActionContainer.Instance.Clear();
//
//            var ass = typeof(AdaSingleQueryAction);
//            foreach (var action in ass.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IAdaAction))))
//            {
//
//                var name = action.Name;
//                if (!throwOnAlreadyExisting)
//                    if (allActions.ContainsKey(name))
//                        continue;
//                allActions.Add(name, action);
//            }
//            Assert.IsEmpty(bad);
//
//        }
//
//        [TestCase]
//        public void NotTooFew()
//        {
//            AdaActionContainer.Instance.Clear();
//            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<AdaSingleQueryAction>(false));
//
//            var exceptions = new Type[]
//                                 {
//                                     typeof(AdaAvCheckNotification),
//                                     typeof(AdaAvInternalError)
//                                 };
//
//            var bad = new List<Type>();
//
//            foreach (var source in 
//                typeof(AdaAvSchemaVersionArchieIndex).Assembly.DefinedTypes
//                .Where(t => typeof(AdaAvCheckNotification).IsAssignableFrom(t))
//                .Except(exceptions)
//                )
//            {
//                var typeName = source.Name;
//                foreach (var ctor in source.GetConstructors())
//                {
//                    if (ctor.GetParameters().Any(p => p.Name == "tagType"))
//                        continue;
//
//                    try
//                    {
//                        var action = AdaActionContainer.Instance.GetAction(source);
//                        //                    Assert.IsNotNull(action);
//                        if (action == null)
//                            bad.Add(source);
//                    }
//                    catch (Exception)
//                    {
//                        bad.Add(source);
//                    }
//                }
//            }
//            Assert.IsEmpty(bad);
//        }
    }
}