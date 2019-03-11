﻿namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;

    #endregion

    // From https://sites.google.com/a/wpfmentor.com/resources/dataresource

    public class DataResource : Freezable
    {
        #region Static

        /// <summary>
        ///     Identifies the <see cref="BindingTarget" /> dependency property.
        /// </summary>
        /// <value>
        ///     The identifier for the <see cref="BindingTarget" /> dependency property.
        /// </value>
        public static readonly DependencyProperty BindingTargetProperty = DependencyProperty.Register("BindingTarget", typeof(object), typeof(DataResource), new UIPropertyMetadata(null));

        #endregion

        #region  Constructors

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the binding target.
        /// </summary>
        /// <value>The binding target.</value>
        public object BindingTarget
        {
            get => GetValue(BindingTargetProperty);
            set => SetValue(BindingTargetProperty, value);
        }

        #endregion

        #region

        /// <summary>
        ///     Makes the instance a clone (deep copy) of the specified <see cref="Freezable" />
        ///     using base (non-animated) property values.
        /// </summary>
        /// <param name="sourceFreezable">
        ///     The object to clone.
        /// </param>
        protected sealed override void CloneCore(Freezable sourceFreezable)
        {
            base.CloneCore(sourceFreezable);
        }

        /// <summary>
        ///     Creates an instance of the specified type using that type's default constructor.
        /// </summary>
        /// <returns>
        ///     A reference to the newly created object.
        /// </returns>
        protected override Freezable CreateInstanceCore()
        {
            return (Freezable) Activator.CreateInstance(GetType());
        }

        #endregion
    }

    public class DataResourceBindingExtension : MarkupExtension
    {
        #region  Fields

        private DataResource mDataResouce;
        private object mTargetObject;
        private object mTargetProperty;

        #endregion

        #region  Constructors

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the data resource.
        /// </summary>
        /// <value>The data resource.</value>
        public DataResource DataResource
        {
            get => mDataResouce;
            set
            {
                if (mDataResouce != value)
                {
                    if (mDataResouce != null) mDataResouce.Changed -= DataResource_Changed;
                    mDataResouce = value;

                    if (mDataResouce != null) mDataResouce.Changed += DataResource_Changed;
                }
            }
        }

        #endregion

        #region

        private object Convert(object obj, Type toType)
        {
            try
            {
                return System.Convert.ChangeType(obj, toType);
            }
            catch (InvalidCastException)
            {
                return obj;
            }
        }

        private void DataResource_Changed(object sender, EventArgs e)
        {
            // Ensure that the bound object is updated when DataResource changes.
            var dataResource = (DataResource) sender;
            var depProp = mTargetProperty as DependencyProperty;

            if (depProp != null)
            {
                var depObj = (DependencyObject) mTargetObject;
                var value = Convert(dataResource.BindingTarget, depProp.PropertyType);
                depObj.SetValue(depProp, value);
            }
            else
            {
                var propInfo = mTargetProperty as PropertyInfo;
                if (propInfo != null)
                {
                    var value = Convert(dataResource.BindingTarget, propInfo.PropertyType);
                    propInfo.SetValue(mTargetObject, value, new object[0]);
                }
            }
        }

        /// <summary>
        ///     When implemented in a derived class, returns an object that is set as the value of the target property for this
        ///     markup extension.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        ///     The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));

            mTargetObject = target.TargetObject;
            mTargetProperty = target.TargetProperty;

            // mTargetProperty can be null when this is called in the Designer.
            Debug.Assert(mTargetProperty != null || DesignerProperties.GetIsInDesignMode(new DependencyObject()));

            if (DataResource.BindingTarget == null && mTargetProperty != null)
            {
                var propInfo = mTargetProperty as PropertyInfo;
                if (propInfo != null)
                    try
                    {
                        return Activator.CreateInstance(propInfo.PropertyType);
                    }
                    catch (MissingMethodException)
                    {
                        // there isn't a default constructor
                    }

                var depProp = mTargetProperty as DependencyProperty;
                if (depProp != null)
                {
                    var depObj = (DependencyObject) mTargetObject;
                    return depObj.GetValue(depProp);
                }
            }

            return DataResource.BindingTarget;
        }

        #endregion
    }
}