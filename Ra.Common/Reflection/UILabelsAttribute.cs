// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UILabelsAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The ui labels attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Reflection
{
    #region Namespace Using

    using System;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    ///     The ui labels attribute.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method
        | AttributeTargets.Parameter | AttributeTargets.Enum)]
    public class UILabelsAttribute : Attribute
    {
        #region  Fields

        /// <summary>
        ///     The description.
        /// </summary>
        private readonly string description;

        /// <summary>
        ///     The grouping.
        /// </summary>
        private readonly string grouping;

        /// <summary>
        ///     The name.
        /// </summary>
        private readonly string name;

        /// <summary>
        ///     The role.
        /// </summary>
        private readonly string role;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UILabelsAttribute" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        /// <param name="role">
        ///     The role.
        /// </param>
        /// <param name="grouping">
        ///     The grouping.
        /// </param>
        public UILabelsAttribute(string name, string description = "", string role = "", string grouping = "")
        {
            this.name = name;
            if (description != string.Empty)
                this.description = description;
            else
                this.description = name;

            this.role = role;
            this.grouping = grouping;
        }

        #endregion

        #region

        /// <summary>
        ///     The get ui attribute.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     The <see cref="UILabelsAttribute" />.
        /// </returns>
        private static UILabelsAttribute GetUIAttribute(ICustomAttributeProvider source)
        {
            var atts = source.GetCustomAttributes(typeof(UILabelsAttribute), false);
            if (atts.Length == 1) return atts[0] as UILabelsAttribute;

            return null;
        }

        /// <summary>
        ///     The get ui description.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetUIDescription(ICustomAttributeProvider source)
        {
            var att = GetUIAttribute(source);
            if (att != null) return att.description;

            return null;
        }

        /// <summary>
        ///     The get ui grouping.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetUIGrouping(ICustomAttributeProvider source)
        {
            var att = GetUIAttribute(source);
            if (att != null) return att.grouping;

            return null;
        }

        public static string GetUIGrouping(string name, Type source)
        {
            return GetUIGrouping(source.GetMember(name).SingleOrDefault());
        }

        /// <summary>
        ///     The get ui name.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetUIName(ICustomAttributeProvider source)
        {
            var att = GetUIAttribute(source);
            if (att != null) return att.name;

            return null;
        }

        public static string GetUIName(string name, Type source)
        {
            return GetUIName(source.GetMember(name).SingleOrDefault());
        }

        /// <summary>
        ///     The get ui role.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetUIRole(ICustomAttributeProvider source)
        {
            var att = GetUIAttribute(source);
            if (att != null) return att.role;

            return null;
        }

        /// <summary>
        ///     The has ui labels.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool hasUILabels(ICustomAttributeProvider source)
        {
            if (GetUIAttribute(source) == null) return false;

            return true;
        }

        #endregion

        // private string subgrouping;
        // private string order;
    }
}