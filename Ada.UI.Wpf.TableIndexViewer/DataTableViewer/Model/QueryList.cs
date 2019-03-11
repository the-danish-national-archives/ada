namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.Model
{
    #region Namespace Using

    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using Properties;

    #endregion

    [DataContract]
    public class QueryList : INotifyPropertyChanged
    {
        #region  Fields

        [DataMember] private Query[] queries = new Query[0];

        #endregion

        #region Properties

        public Query[] Queries
        {
            get => queries;

            set
            {
                queries = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}