namespace Ra.Common.Wpf
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Controls;

    #endregion

    public class StupidComboBox : ComboBox
    {
        #region Static

        /// <summary>
        ///     The <see cref="Child" /> dependency property's name.
        /// </summary>
        public const string ChildPropertyName = "Child";

        /// <summary>
        ///     The <see cref="Popup" /> dependency property's name.
        /// </summary>
        public const string PopupPropertyName = "Popup";

        /// <summary>
        ///     Identifies the <see cref="Child" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(
            ChildPropertyName,
            typeof(object),
            typeof(StupidComboBox),
            new UIPropertyMetadata(null));

        /// <summary>
        ///     Identifies the <see cref="Popup" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupProperty = DependencyProperty.Register(
            PopupPropertyName,
            typeof(object),
            typeof(StupidComboBox),
            new UIPropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the value of the <see cref="Child" />
        ///     property. This is a dependency property.
        /// </summary>
        public object Child
        {
            get => GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }

        /// <summary>
        ///     Gets or sets the value of the <see cref="Popup" />
        ///     property. This is a dependency property.
        /// </summary>
        public object Popup
        {
            get => GetValue(PopupProperty);
            set => SetValue(PopupProperty, value);
        }

        #endregion
    }
}