// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHTransformer.cs" company="">
//   
// </copyright>
// <summary>
//   The nh transformers.
// http://adrianphinney.com/post/18900251364/nhibernate-raw-sql-and-dynamic-result-sets
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Repository.NHibernate
{
    #region Namespace Using

    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using global::NHibernate;
    using global::NHibernate.Transform;

    #endregion

    /// <summary>
    ///     The nh transformers.
    /// </summary>
    public static class NhTransformers
    {
        #region Static

        /// <summary>
        ///     The expando object.
        /// </summary>
        public static readonly IResultTransformer ExpandoObject;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes static members of the <see cref="NhTransformers" /> class.
        /// </summary>
        static NhTransformers()
        {
            ExpandoObject = new ExpandoObjectResultSetTransformer();
        }

        #endregion

        #region Nested type: ExpandoObjectResultSetTransformer

        /// <summary>
        ///     The expando object result set transformer.
        /// </summary>
        private class ExpandoObjectResultSetTransformer : IResultTransformer
        {
            #region IResultTransformer Members

            /// <summary>
            ///     The transform list.
            /// </summary>
            /// <param name="collection">
            ///     The collection.
            /// </param>
            /// <returns>
            ///     The <see cref="IList" />.
            /// </returns>
            public IList TransformList(IList collection)
            {
                return collection;
            }

            /// <summary>
            ///     The transform tuple.
            /// </summary>
            /// <param name="tuple">
            ///     The tuple.
            /// </param>
            /// <param name="aliases">
            ///     The aliases.
            /// </param>
            /// <returns>
            ///     The <see cref="object" />.
            /// </returns>
            public object TransformTuple(object[] tuple, string[] aliases)
            {
                var expando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>) expando;
                for (var i = 0; i < tuple.Length; i++)
                {
                    var alias = aliases[i];
                    if (alias != null) dictionary[alias] = tuple[i];
                }

                return expando;
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    ///     The n hibernate extensions.
    /// </summary>
    public static class NHibernateExtensions
    {
        #region

        /// <summary>
        ///     The dynamic list.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public static IEnumerable<dynamic> DynamicList(this IQuery query)
        {
            return query.SetResultTransformer(NhTransformers.ExpandoObject).List<dynamic>();
        }

        #endregion
    }
}