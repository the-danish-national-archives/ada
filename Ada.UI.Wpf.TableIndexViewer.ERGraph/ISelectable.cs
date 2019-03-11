namespace Ada.UI.Wpf.TableIndexViewer.ERGraph
{
    #region Namespace Using

    using System;

    #endregion

    public interface ISelectable
    {
        #region Properties

        bool Selected { get; set; }

        #endregion

        #region

        event Action SelectedChange;

        #endregion
    }
}