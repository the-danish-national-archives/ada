namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Windows.Markup;

    #endregion

    [MarkupExtensionReturnType(typeof(Type))]
    public class Type2Extension : TypeExtension
    {
        #region  Constructors

        public Type2Extension()
        {
        }

        public Type2Extension(string typeName)
        {
            TypeName = typeName;
        }

        #endregion

        #region

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var typeResolver = (IXamlTypeResolver) serviceProvider.GetService(typeof(IXamlTypeResolver));
            var sepindex = TypeName.IndexOf('+');
            if (sepindex < 0)
                return typeResolver.Resolve(TypeName);


            var outerType = typeResolver.Resolve(TypeName.Substring(0, sepindex));
            return outerType.Assembly.GetType(outerType.FullName + "+" + TypeName.Substring(sepindex + 1));
        }

        #endregion
    }
}