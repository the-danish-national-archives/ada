namespace Ada.UI.Wpf.TableIndexViewer.ERGraph
{
    #region Namespace Using

    using System;

    #endregion

    public interface ISelectionDetector
    {
        #region

        bool IsSelected(object o);

        event Action SelectionChanged;

        #endregion
    }
}