namespace Ada.Test.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System.Data;
    using Ada.UI.Wpf.ProgressManagement.ViewModel;
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using JetBrains.Annotations;
    using Log;
    //using Microsoft.Practices.ServiceLocation;

    #endregion


    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region  Constructors

//        private readonly AdaTestLog testLog;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(AdaTestLog testLog)
        {
//            this.testLog = testLog;
            ProgressManagementViewModel =
                new ProgressManagementViewModel(ServiceLocator.Current.GetInstance<IDbConnection>())
                    {TestLog = testLog};
        }

        #endregion

        #region Properties

        [NotNull]
        public ProgressManagementViewModel ProgressManagementViewModel { get; }

        #endregion
    }
}