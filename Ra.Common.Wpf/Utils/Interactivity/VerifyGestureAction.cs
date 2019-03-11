namespace Ra.Common.Wpf.Utils.Interactivity
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Markup;
    using TriggerAction = System.Windows.Interactivity.TriggerAction;

    #endregion

    [ContentProperty("Actions")]
    public class VerifyGestureAction : TriggerAction<FrameworkElement>
    {
        #region Static

        public static readonly DependencyProperty MouseGestureProperty = DependencyProperty.Register("MouseGesture", typeof(MouseGesture), typeof(VerifyGestureAction), new PropertyMetadata(default(InputGesture)));

        private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Actions",
            typeof(FreezableCollection<TriggerAction>),
            typeof(VerifyGestureAction),
            new FrameworkPropertyMetadata(new FreezableCollection<TriggerAction>()));

        public static DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;

        #endregion

        #region  Constructors

        public VerifyGestureAction()
        {
            SetValue(ActionsPropertyKey, new FreezableCollection<TriggerAction>());
        }

        #endregion

        #region Properties

        public FreezableCollection<TriggerAction> Actions => (FreezableCollection<TriggerAction>) GetValue(ActionsProperty);

        public MouseGesture MouseGesture
        {
            get => (MouseGesture) GetValue(MouseGestureProperty);
            set => SetValue(MouseGestureProperty, value);
        }

        #endregion

        #region

        // Private Implementation.

        protected override void Invoke(object parameter)
        {
            if (MouseGesture?.Matches(null, parameter as InputEventArgs) ?? false)
                foreach (var action in Actions)
                    action.Invoke();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            foreach (var action in Actions) action.Attach(AssociatedObject);
        }

        #endregion
    }
}