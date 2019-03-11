namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Windows.Markup;

    #endregion

    public abstract class BaseConverter : MarkupExtension
    {
        #region

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }
}