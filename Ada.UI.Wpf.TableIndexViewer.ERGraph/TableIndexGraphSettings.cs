namespace Ada.UI.Wpf.TableIndexViewer.ERGraph
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.CommandWpf;
    using GraphX.PCL.Common.Enums;
    using JetBrains.Annotations;
    using Models;

    #endregion

    public class TableIndexGraphSettings : INotifyPropertyChanged
    {
        #region EdgeRoutingEnum enum

        public enum EdgeRoutingEnum
        {
            [Description("EdgeRoutingEnumNone")] None,

            [Description("EdgeRoutingEnumPathFinder")]
            PathFinder,

            [Description("EdgeRoutingEnumSimpleER")]
            SimpleER,

            [Description("EdgeRoutingEnumBundling")]
            Bundling
        }

        #endregion

        #region LayoutEnum enum

        public enum LayoutEnum
        {
            [Description("LayoutBoundedFR")] BoundedFR,
            [Description("LayoutCircular")] Circular,
            [Description("LayoutCompoundFDP")] CompoundFDP,

            [Description("LayoutEfficientSugiyama")]
            EfficientSugiyama,
            [Description("LayoutSugiyama")] Sugiyama,
            [Description("LayoutFR")] FR,
            [Description("LayoutISOM")] ISOM,
            [Description("LayoutKK")] KK,
            [Description("LayoutLinLog")] LinLog,
            [Description("LayoutTree")] Tree,
            [Description("LayoutSimpleRandom")] SimpleRandom
        }

        #endregion

        #region OverlapRemovalEnum enum

        public enum OverlapRemovalEnum
        {
            [Description("OverlapRemovalEnumFSA")] FSA,

            [Description("OverlapRemovalEnumOneWayFSA")]
            OneWayFSA,

            [Description("OverlapRemovalEnumNone")]
            None
        }

        #endregion

        #region  Fields

        private bool _edgeCurvingEnabled = true;

        private EdgeRoutingEnum _edgeRouting = EdgeRoutingEnum.SimpleER;

        private LayoutEnum _layout = LayoutEnum.KK;

        private OverlapRemovalEnum _overlapRemoval = OverlapRemovalEnum.FSA;

        private RelayCommand _redrawLayoutCommand;

        #endregion

        #region Properties

        public bool EdgeCurvingEnabled
        {
            get => _edgeCurvingEnabled;
            set
            {
                _edgeCurvingEnabled = value;
                OnPropertyChanged();
            }
        }

        public EdgeRoutingEnum EdgeRouting
        {
            get => _edgeRouting;
            set
            {
                _edgeRouting = value;
                OnPropertyChanged();
            }
        }

        public EdgeRoutingAlgorithmTypeEnum EdgeRoutingAlgorithm => (EdgeRoutingAlgorithmTypeEnum) Enum.Parse(typeof(EdgeRoutingAlgorithmTypeEnum), EdgeRouting.ToString());

        public LayoutEnum Layout
        {
            get => _layout;
            set
            {
                _layout = value;
                OnPropertyChanged();
            }
        }

        public LayoutAlgorithmTypeEnum LayoutAlgorithm => (LayoutAlgorithmTypeEnum) Enum.Parse(typeof(LayoutAlgorithmTypeEnum), Layout.ToString());

        public OverlapRemovalEnum OverlapRemoval
        {
            get => _overlapRemoval;
            set
            {
                _overlapRemoval = value;
                OnPropertyChanged();
            }
        }

        public OverlapRemovalAlgorithmTypeEnum OverlapRemovalAlgorithm => (OverlapRemovalAlgorithmTypeEnum) Enum.Parse(typeof(OverlapRemovalAlgorithmTypeEnum), OverlapRemoval.ToString());

        public ICommand RedrawLayoutCommand
        {
            get { return _redrawLayoutCommand ?? (_redrawLayoutCommand = new RelayCommand(() => RedrawLayoutRequested?.Invoke())); }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        public void InvokeRefreshSelected()
        {
            RefreshSelected?.Invoke();
        }

        public void InvokeTableWantsFocus(TableIndexVertex table)
        {
            TableWantsFocus?.Invoke(table);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event Action RedrawLayoutRequested;

        public event Action RefreshSelected;

        public event Action<TableIndexVertex> TableWantsFocus;

        #endregion
    }
}