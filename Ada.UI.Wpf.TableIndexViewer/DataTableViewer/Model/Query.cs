namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.Model
{
    #region Namespace Using

    using System.Runtime.Serialization;

    #endregion

    [DataContract]
    public class Query
    {
        #region  Fields

//        [DataMember]
//        public string Name;

        [DataMember] public string Value;

        #endregion
    }
}