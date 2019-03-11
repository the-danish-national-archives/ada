#region Header

// Author 
// Created 20

#endregion

namespace Ra.Common.Types
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;

    #endregion

    // From http://stackoverflow.com/questions/7252186/switch-case-on-type-c-sharp/7301514#7301514
    public class TypeSwitch
    {
        #region  Fields

        private readonly Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();

        #endregion

        #region

        public TypeSwitch Case<T>(Action<T> action)
        {
            matches.Add(typeof(T), x => action((T) x));
            return this;
        }

        public void Switch(object x)
        {
            matches[x.GetType()](x);
        }

        #endregion
    }

    // Example
//    public static void TestTypeSwitch()
//    {
//        var ts = new TypeSwitch()
//            .Case((int x) => Console.WriteLine("int"))
//            .Case((bool x) => Console.WriteLine("bool"))
//            .Case((string x) => Console.WriteLine("string"));
//
//        ts.Switch(42);
//        ts.Switch(false);
//        ts.Switch("hello");
//    }
}