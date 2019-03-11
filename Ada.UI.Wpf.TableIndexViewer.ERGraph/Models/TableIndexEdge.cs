namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Models
{
    #region Namespace Using

    using System.ComponentModel;
    using System.Linq;
    using GraphX.PCL.Common.Models;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexEdge : EdgeBase<TableIndexVertex>, INotifyPropertyChanged, ISelectableConnector
    {
        #region  Fields

        /// <summary>
        ///     Node main description (header)
        /// </summary>
        private string _text;

        #endregion

        #region  Constructors

        public TableIndexEdge(TableIndexVertex source, TableIndexVertex target, ForeignKey foreignKey, double weight = 1)
            : base(source, target, weight)
        {
            ForeignKey = foreignKey;
            Angle = 90;
            Text = $"{foreignKey.Name}";
        }

        public TableIndexEdge()
            : base(null, null, 1)
        {
            Angle = 90;
        }

        #endregion

        #region Properties

        public double Angle { get; set; }

        public bool ArrowTarget { get; set; }
        public ForeignKey ForeignKey { get; }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public string ToolTipText => $"{ForeignKey.References.Select(r => r.Column).SmartToString()}"
                                     + $" => {ForeignKey.References.Select(r => r.Referenced).SmartToString()}";

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ISelectableConnector Members

        public ISelectable In => Source;

        public ISelectable Out => Target;

        #endregion

        #region

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}