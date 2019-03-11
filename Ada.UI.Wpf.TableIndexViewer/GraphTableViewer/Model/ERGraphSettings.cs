namespace Ada.UI.Wpf.TableIndexViewer.GraphTableViewer.Model
{
    #region Namespace Using

    using ERGraph;

    #endregion

    public class ERGraphSettings
    {
        #region  Fields

        #endregion

        #region Properties

        public TableIndexGraphSettings GraphSettings { get; } = new TableIndexGraphSettings();

        public ERGraphVisualSettings VisualSettings { get; } = new ERGraphVisualSettings();

        #endregion
    }
}