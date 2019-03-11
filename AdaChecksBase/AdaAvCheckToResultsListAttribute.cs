namespace Ada.ChecksBase
{
    #region Namespace Using

    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using Log.Entities;

    #endregion

    [AttributeUsage(
        AttributeTargets.Class
//        ,AllowMultiple = true
    )]
    public class AdaAvCheckToResultsListAttribute : Attribute
    {
        #region  Fields

        [NotNull] private readonly string propertyName;
        private readonly char seperator;

        #endregion

        #region  Constructors

        public AdaAvCheckToResultsListAttribute([NotNull] string propertyName, char seperator = ';')
        {
            this.propertyName = propertyName;
            this.seperator = seperator;
        }

        #endregion

        #region

        public Func<string[]> GetListGenerator([NotNull] LogEntry logEntry)
        {
//            if (!method.GetCustomAttributes(GetType()).Any())
//                throw new InvalidOperationException();
//            
//            if (method.ReturnType != typeof(string))
//                throw new InvalidOperationException();
//            
//            if (!method.IsStatic)
//                throw new InvalidOperationException();
//            
//            var parameterInfos = method.GetParameters();
//            
//            if (parameterInfos.Length != 0)
//                throw new InvalidOperationException();
//
//            return (string) method.Invoke(null, null);

            return () => { return logEntry.EntryTags.FirstOrDefault(t => t.TagType == propertyName)?.TagText.Split(seperator); };
        }

        #endregion

        //        /// <summary>
        //        /// Given a method like 
        //        /// <code>
        //        /// [AdaAvCheckToAvQuery(nameof(ConstraintName))] public static string GetAvQuery(PrimaryKey pk)
        //        /// </code> returns
        //        /// a converter method like 
        //        /// <c>
        //        /// Func&lt;Func&lt;string, PrimaryKey&gt;, string&gt;
        //        ///   c => GetAvQuery(logEntry.EntryTags.FirstOrDefault(t => t.TagType == "ConstraintName")?.TagText)</c>.
        //        /// </summary>
        //        public Delegate CreateFromLogEntryToAvQueryConverter([NotNull] MethodInfo method, [NotNull] LogEntry logEntry)
        //        {
        //            if (!method.GetCustomAttributes(GetType()).Any())
        //                throw new InvalidOperationException();
        //
        //            if (Parameters.Length > 1)
        //                throw new NotImplementedException();
        //
        //            if (method.ReturnType != typeof(string))
        //                throw new InvalidOperationException();
        //
        //            if (!method.IsStatic)
        //                throw new InvalidOperationException();
        //
        //            var parameterInfos = method.GetParameters();
        //
        //            if (parameterInfos.Length != Parameters.Length)
        //                throw new InvalidOperationException();
        //
        //            if (Parameters.Length == 0)
        //                throw new InvalidOperationException();
        //
        //            var toType = parameterInfos[0].ParameterType;
        //
        //// var declaringType = method.DeclaringType;
        //
        //// var fromProperty = declaringType?.GetProperty(Parameters[0]);
        //
        //// if (fromProperty == null)
        //
        //// throw new InvalidOperationException();
        //            var fromType = typeof(string); // type from logEntry.TagText
        //
        //            var funcType = typeof(Func<,>);
        //
        //            var parameter1Converter = funcType.MakeGenericType(fromType, toType);
        //
        //            Func<object, string> converterFunc = (object o) =>
        //                {
        //                    if (!parameter1Converter.IsInstanceOfType(o))
        //                        throw new InvalidOperationException();
        //
        //                    var parameter1Value = parameter1Converter
        //                        .GetMethod("Invoke")?
        //                        .Invoke(
        //                            o, 
        //                            new object[]
        //                                {
        //                                    logEntry.EntryTags.FirstOrDefault(t => t.TagType == Parameters[0])?.TagText
        //                                });
        //
        //                    var result = method.Invoke(null, new[] { parameter1Value });
        //
        //                    return result as string;
        //                };
        //
        //            var expArg = Expression.Parameter(parameter1Converter);
        //
        //            var expInvoke = Expression.Invoke(Expression.Constant(converterFunc), expArg);
        //
        //            var res = Expression.Lambda(expInvoke, expArg).Compile();
        //
        //            return res;
        //        }
    }
}