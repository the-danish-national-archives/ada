namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Resources;
    using System.Windows.Markup;

    #endregion

    [MarkupExtensionReturnType(typeof(IEnumerable<EnumerationMember>))]
    public class EnumerationExtension : MarkupExtension
    {
        #region  Fields

        private Type _enumType;

        #endregion

        #region  Constructors

        public EnumerationExtension()
        {
        }

        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            EnumType = enumType;
        }

        #endregion

        #region Properties

        [ConstructorArgument("enumType")]
        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        [ConstructorArgument("resourceManager")]
        public ResourceManager ResourceManager { get; set; }

        #endregion

        #region

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            if (descriptionAttribute == null)
                return enumValue.ToString();

            var resource = ResourceManager?.GetString(descriptionAttribute.Description);

            return resource ?? descriptionAttribute.Description;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null)
                return null;

            var enumValues = Enum.GetValues(EnumType);

            return (
                from object enumValue in enumValues
                select new EnumerationMember
                {
                    Value = enumValue,
                    Description = GetDescription(enumValue)
                }).ToArray();
        }

        #endregion

        #region Nested type: EnumerationMember

        public class EnumerationMember
        {
            #region Properties

            public string Description { get; set; }
            public object Value { get; set; }

            #endregion
        }

        #endregion
    }
}