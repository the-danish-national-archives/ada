﻿namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Media;

    #endregion

    public static class UIHelper
    {
        #region

        /// <summary>
        ///     Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>
        ///     The first parent item that matches the submitted type parameter.
        ///     If not matching item can be found, a null reference is being returned.
        /// </returns>
        public static T FindVisualParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            // get parent item
            var parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for
            var parent = parentObject as T;
            if (parent != null)
                return parent;
            return FindVisualParent<T>(parentObject);
        }

        #endregion
    }
}