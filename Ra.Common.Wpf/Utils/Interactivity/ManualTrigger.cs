namespace Ra.Common.Wpf.Utils.Interactivity
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Interactivity;

    #endregion

    /// <summary>
    ///     A trigger that may be invoked from code.
    /// </summary>
    public class ManualTrigger : TriggerBase<DependencyObject>
    {
        #region

        /// <summary>
        ///     Invokes the trigger's actions.
        /// </summary>
        /// <param name="parameter">The parameter value.</param>
        public void Invoke(object parameter)
        {
            InvokeActions(parameter);
        }

        #endregion
    }
}