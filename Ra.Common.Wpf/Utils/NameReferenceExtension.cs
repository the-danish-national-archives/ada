#region Header

// Author 
// Created 13

#endregion

namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Windows.Markup;
    using System.Xaml;

    #endregion

    // From http://stackoverflow.com/questions/26468845/xreference-cannot-find-xname-in-design-mode-compiles-and-runs-fine
    [ContentProperty("Name")]
    public class NameReferenceExtension : MarkupExtension
    {
        #region  Constructors

        public NameReferenceExtension()
        {
        }

        public NameReferenceExtension(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        [ConstructorArgument("name")]
        public string Name { get; set; }

        #endregion

        #region

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            var xamlNameResolver = serviceProvider.GetService(typeof(IXamlNameResolver))
                as IXamlNameResolver;
            if (xamlNameResolver == null)
                return null; // fail silently

            if (string.IsNullOrEmpty(Name))
                throw new InvalidOperationException(
                    "Name is required when using NameReference.");

            var resolved = xamlNameResolver.Resolve(Name);
            if (resolved == null)
                resolved = xamlNameResolver.GetFixupToken(new[] {Name}, true);

            return resolved;
        }

        #endregion
    }
}