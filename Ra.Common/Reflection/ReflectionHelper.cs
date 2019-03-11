// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The reflection helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Reflection
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    #endregion

    /// <summary>
    ///     The reflection helper.
    /// </summary>
    public static class ReflectionHelper
    {
        #region

        /// <summary>
        ///     The get shared base class.
        /// </summary>
        /// <param name="types">
        ///     The types.
        /// </param>
        /// <returns>
        ///     The <see cref="Type" />.
        /// </returns>
        public static Type GetSharedBaseClass(params Type[] types)
        {
            if (types.Length == 0) return typeof(object);

            var t = types[0];
            for (var i = 1; i < types.Length; ++i)
                if (types[i].IsAssignableFrom(t))
                    t = types[i];
                else
                    while (!t.IsAssignableFrom(types[i]))
                        t = t.BaseType;

            return t;
        }

        /// <summary>
        ///     The get shared interfaces.
        /// </summary>
        /// <param name="types">
        ///     The types.
        /// </param>
        /// <returns>
        ///     The <see cref="Type[]" />.
        /// </returns>
        public static Type[] GetSharedInterfaces(params Type[] types)
        {
            if (types.Length == 0) return new Type[0];

            var sharedInterfaces = new List<Type>();
            sharedInterfaces.AddRange(types[0].GetInterfaces());

            for (var i = 1; i < types.Length; ++i)
            for (var j = sharedInterfaces.Count - 1; j > 0; --j)
                if (types[i].GetInterface(sharedInterfaces[j].Name) == null)
                    sharedInterfaces.Remove(sharedInterfaces[j]);

            return sharedInterfaces.ToArray();
        }

        /// <summary>
        ///     The get shared properties.
        /// </summary>
        /// <param name="types">
        ///     The types.
        /// </param>
        /// <returns>
        ///     The <see cref="PropertyInfo[]" />.
        /// </returns>
        public static PropertyInfo[] GetSharedProperties(params Type[] types)
        {
            if (types.Length == 0) return new PropertyInfo[0];

            var sharedContracts = new List<Type>();
            sharedContracts.Add(GetSharedBaseClass(types));
            if (sharedContracts[0] != types[0]) sharedContracts.AddRange(GetSharedInterfaces(types));

            var sharedProperties = new List<PropertyInfo>();
            foreach (var contract in sharedContracts)
            {
                var props =
                    contract.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                    if (!sharedProperties.Contains(prop))
                        sharedProperties.Add(prop);
            }

            return sharedProperties.ToArray();
        }

        /// <summary>
        ///     The get sub classes.
        /// </summary>
        /// <param name="baseClassType">
        ///     The base class type.
        /// </param>
        /// <param name="asm">
        ///     The asm.
        /// </param>
        /// <returns>
        ///     The <see cref="Type[]" />.
        /// </returns>
        public static Type[] GetSubClasses(Type baseClassType, Assembly asm = null)
        {
            if (asm == null) asm = Assembly.GetExecutingAssembly();

            var subClasses = new List<Type>();
            foreach (var type in asm.GetTypes())
                if (type.IsSubclassOf(baseClassType))
                    subClasses.Add(type);

            return subClasses.ToArray();
        }

        /// <summary>
        ///     The update shared properties.
        /// </summary>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="props">
        ///     The props.
        /// </param>
        /// <param name="attributeFilter">
        ///     The attribute filter.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public static int UpdateSharedProperties
        (
            object target,
            object source,
            PropertyInfo[] props,
            Type attributeFilter = null)
        {
            var updateCount = 0;
            foreach (var pi in props)
                if (pi.CanRead && pi.CanWrite
                               && (attributeFilter == null || attributeFilter != null && pi.IsDefined(attributeFilter, true)))
                {
                    var oldValue = pi.GetValue(target, null);
                    var newValue = pi.GetValue(source, null);

                    if (oldValue != null && newValue != null && !oldValue.Equals(newValue)
                        || oldValue == null && newValue != null)
                    {
                        pi.SetValue(target, newValue, null);
                        updateCount++;
                    }
                }

            return updateCount;
        }

        #endregion
    }
}