/*
 * These tests are no longer good
 * 
 * NotTooMany(), makes no sense now that multiple 
 * actions can result in the same check running
 * 
 * NotTooFew(), makes no sense as some checks might be disabled for various reasons
 * 
 * 
 * */
//namespace Ada.Test.UnitTests.Actions
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Reflection;
//    using Action64.IngestActions;
//    using Ada.ActionBase;
//    using Ada.Actions;
//    using Ada.Checks.ArchiveIndex;
//    using Ada.ChecksBase;
//
//    using NUnit.Framework;
//
//    [TestFixture(Category = "ChecksAccountedFor")]
//    public class ChecksAccountedFor
//    {
//        [TestCase]
//        public void NotTooMany()
//        {
//            AdaActionContainer.Instance.Clear();
//
//
//            var bad = new List<Type>();
//            Dictionary<Type, Type> checkToActions = new Dictionary<Type, Type>();
//
//            foreach (var source in typeof(AdaSingleQueryAction).Assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IAdaAction)))
//                    .SelectMany(t => t.GetCustomAttributes<ReportsChecksAttribute>().Select(att => new { Type = t, Att = att })))
//            {
//                foreach (var check in source.Att.Checks)
//                {
//                    if (checkToActions.ContainsKey(check))
//                    {
//                        bad.Add(check);
//                        continue;
//                    }
//                    checkToActions.Add(check, source.Type);
//
//                }
//            }
//            Assert.IsEmpty(bad);
//
//        }
//
//        [TestCase]
//        public void NotTooFew()
//        {
//            AdaActionContainer.Instance.Clear();
//            Assert.DoesNotThrow(() => AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>(false));
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
//                        var action = AdaActionContainer.Instance.GetAction("source");
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
//    }
//}

