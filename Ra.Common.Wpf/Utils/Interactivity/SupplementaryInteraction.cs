namespace Ra.Common.Wpf.Utils.Interactivity
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Interactivity;
    using TriggerBase = System.Windows.Interactivity.TriggerBase;

    #endregion

    public class Behaviors : List<Behavior>
    {
    }

    public class Triggers : List<TriggerBase>
    {
    }

    public static class SupplementaryInteraction
    {
        #region Static

        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("Behaviors", typeof(Behaviors), typeof(SupplementaryInteraction), new UIPropertyMetadata(null, OnPropertyBehaviorsChanged));

        public static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached("Triggers", typeof(Triggers), typeof(SupplementaryInteraction), new UIPropertyMetadata(null, OnPropertyTriggersChanged));

        #endregion

        #region

        public static Behaviors GetBehaviors(DependencyObject obj)
        {
            return (Behaviors) obj.GetValue(BehaviorsProperty);
        }

        public static Triggers GetTriggers(DependencyObject obj)
        {
            return (Triggers) obj.GetValue(TriggersProperty);
        }

        private static void OnPropertyBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behaviors = Interaction.GetBehaviors(d);
            foreach (var behavior in e.NewValue as Behaviors) behaviors.Add(behavior);
        }

        private static void OnPropertyTriggersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var triggers = Interaction.GetTriggers(d);
            if (e.NewValue != null)
                foreach (var trigger in e.NewValue as Triggers)
                    triggers.Add(trigger);
        }

        public static void SetBehaviors(DependencyObject obj, Behaviors value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        public static void SetTriggers(DependencyObject obj, Triggers value)
        {
            obj.SetValue(TriggersProperty, value);
        }

        #endregion
    }
}