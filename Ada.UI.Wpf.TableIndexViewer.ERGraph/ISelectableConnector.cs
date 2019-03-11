namespace Ada.UI.Wpf.TableIndexViewer.ERGraph
{
    public interface ISelectableConnector
    {
        #region Properties

        ISelectable In { get; }

        ISelectable Out { get; }

        #endregion
    }
}