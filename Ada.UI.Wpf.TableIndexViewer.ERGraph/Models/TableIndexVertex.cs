namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Models
{
    #region Namespace Using

    using System;
    using GraphX.PCL.Common.Models;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexVertex : VertexBase, ISelectable
    {
        #region  Fields

        private bool _selected;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Default constructor for this class
        ///     (required for serialization).
        /// </summary>
        public TableIndexVertex() : this(string.Empty)
        {
        }

        public TableIndexVertex(string text = "")
        {
            Name = string.IsNullOrEmpty(text) ? "New Vertex" : text;
        }

        public TableIndexVertex(Table table)
        {
            Table = table;
            Name = table.Name;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public Table Table { get; }

        #endregion

        #region ISelectable Members

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected == value)
                    return;
                _selected = value;
                OnSelectedChange();
            }
        }

        public event Action SelectedChange;

        #endregion

        #region

        protected virtual void OnSelectedChange()
        {
            SelectedChange?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}