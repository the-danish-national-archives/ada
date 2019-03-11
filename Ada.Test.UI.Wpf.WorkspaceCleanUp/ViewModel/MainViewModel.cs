namespace Ada.Test.UI.Wpf.WorkspaceCleanUp.ViewModel
{
    #region Namespace Using

    using Ada.UI.Wpf.WorkspaceCleanUp.ViewModel;
    using GalaSoft.MvvmLight;

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
    public class MainViewModel2 : ViewModelBase
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel2()
        {
            WorkspaceCleanUpViewModel = new WorkspaceCleanUpViewModel();
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        #endregion

        #region Properties

        public string Name => "Test window";


        public WorkspaceCleanUpViewModel WorkspaceCleanUpViewModel { get; }

        #endregion
    }
}