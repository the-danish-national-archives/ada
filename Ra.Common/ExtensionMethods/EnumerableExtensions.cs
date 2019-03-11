// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   Extensions for classes implementing the IEnumerable interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.ExtensionMethods
{
    #region Namespace Using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion

    /// <summary>
    ///     Extensions for classes implementing the IEnumerable interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region ReturnValueType enum

        /// <summary>
        ///     The type of values to be returned from the collection.
        /// </summary>
        public enum ReturnValueType
        {
            /// <summary>
            ///     The key.
            /// </summary>
            Key,

            /// <summary>
            ///     The value.
            /// </summary>
            Value,

            /// <summary>
            ///     Both values with equal in between
            /// </summary>
            Both
        }

        #endregion

        #region

//        public static TSource AggregateWhile<TSource>(
//               this IEnumerable<TSource> source,
//               Func<TSource, TSource, TSource> func,
//               Func<TSource, bool> predicate)
//        {
//            if (source == null)
//                throw new ArgumentNullException(nameof(source));
//
//            if (func == null)
//                throw new ArgumentNullException(nameof(func));
//
//            if (predicate == null)
//                throw new ArgumentNullException(nameof(predicate));
//            
//            using (IEnumerator<TSource> e = source.GetEnumerator())
//            {
//                TSource result = e.Current;
//                TSource tmp = default(TSource);
//                while (e.MoveNext() && predicate(tmp = func(result, e.Current)))
//                    result = tmp;
//                return result;
//            }
//        }


        public static TSource AggregateWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func, Func<TSource, bool> pred)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (func == null) throw new ArgumentNullException(nameof(pred));
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext()) throw new InvalidOperationException($"{nameof(source)} contains no elements.");
                var result = e.Current;
                while (e.MoveNext() && pred(result = func(result, e.Current)))
                    ;
                return result;
            }
        }

        public static TAccumulate AggregateWhile<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, bool> pred)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (func == null) throw new ArgumentNullException(nameof(pred));
            using (var e = source.GetEnumerator())
            {
                var result = seed;
                var tmp = default(TAccumulate);
                while (e.MoveNext() && pred(result = func(result, e.Current)))
                    ;
                return result;
            }

//            TAccumulate result = seed;
//            TSource tmp = default(TSource);
//            while (e.MoveNext() && pred(tmp = func(result, e.Current)))
//                foreach (TSource element in source) result = func(result, element);
//            return result;
        }

        public static TResult AggregateWhile<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, bool> pred, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (func == null) throw new ArgumentNullException(nameof(pred));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            using (var e = source.GetEnumerator())
            {
                var result = seed;
                var tmp = default(TAccumulate);
                while (e.MoveNext() && pred(result = func(result, e.Current)))
                    ;
                return resultSelector(result);
            }
        }


        public static IEnumerable<T> AsEnumerableOrEmpty<T>
        (
            this IEnumerable<T> list)
        {
            return list ?? Enumerable.Empty<T>();
        }

        public static IEnumerable AsEnumerableOrEmpty
        (
            this IEnumerable list)
        {
            return list ?? Enumerable.Empty<object>();
        }


        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue res;
            if (key == null)
                return default(TValue);
            return dic.TryGetValue(key, out res) ? res : default(TValue);
        }


        /// <summary>
        ///     The smart to string.
        /// </summary>
        /// <param name="collection">
        ///     The collection.
        /// </param>
        /// <param name="separator">
        ///     The separator.
        /// </param>
        /// <param name="quoted">
        ///     The quoted.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string SmartToString
        (
            this IEnumerable<string> collection,
            string separator = ",",
            bool quoted = false,
            string nullReplacement = "NULL")
        {
            var counter = 1;
            var sb = new StringBuilder();
            foreach (var s in collection)
            {
                if (quoted && s != null)
                    sb.Append("'" + s + "'");
                else
                    sb.Append(s ?? nullReplacement);

                if (counter < collection.Count()) sb.Append(separator);

                counter++;
            }

            return sb.ToString();
        }

        /// <summary>
        ///     The smart to string.
        /// </summary>
        /// <param name="collection">
        ///     The collection.
        /// </param>
        /// <param name="rv">
        ///     The return value
        /// </param>
        /// <param name="separator">
        ///     The separator.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string SmartToString
        (
            this IEnumerable<KeyValuePair<string, string>> collection,
            ReturnValueType rv = ReturnValueType.Both,
            string separator = ",")
        {
            var sb = new StringBuilder();

            var first = true;
            foreach (var kvp in collection)
            {
                if (first)
                    first = false;
                else
                    sb.Append(separator);

                switch (rv)
                {
                    case ReturnValueType.Key:
                        sb.Append(kvp.Key);
                        break;
                    case ReturnValueType.Value:
                        sb.Append(kvp.Value);
                        break;
                    case ReturnValueType.Both:
                        sb.Append(kvp.Key).Append("=").Append(kvp.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rv), rv, null);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Wraps this object instance into an IEnumerable&lt;T&gt;
        ///     consisting of a single item.
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="item"> The instance that will be wrapped. </param>
        /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }


        /// <summary>
        ///     Wraps this object instance into an IEnumerable&lt;T&gt;
        ///     consisting of a single item.
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="item"> The instance that will be wrapped. </param>
        /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
        public static IEnumerable<T> YieldOrEmpty<T>(this T item)
        {
            if (item != null)
                yield return item;
        }

        #endregion
    }
}