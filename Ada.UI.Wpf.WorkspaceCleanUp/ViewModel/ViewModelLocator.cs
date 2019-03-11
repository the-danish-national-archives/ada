/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Ada.UI.Wpf.WorkspaceCleanUp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

namespace Ada.UI.Wpf.WorkspaceCleanUp.ViewModel
{
    #region Namespace Using

    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Ioc;
    //using Microsoft.Practices.ServiceLocation;

    #endregion

    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    // Create run time view services and models
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            SimpleIoc.Default.Register<WorkspaceCleanUpViewModel>();
            SimpleIoc.Default.Register<SetDBLocationViewModel>();
            SimpleIoc.Default.Register<CleanUpCurrentViewModel>();
        }

        #endregion

        #region Properties

        public CleanUpCurrentViewModel CleanUpCurrentViewModel => ServiceLocator.Current.GetInstance<CleanUpCurrentViewModel>();

        public SetDBLocationViewModel SetDBLocationViewModel => ServiceLocator.Current.GetInstance<SetDBLocationViewModel>();

        public WorkspaceCleanUpViewModel WorkspaceCleanUpViewModel => ServiceLocator.Current.GetInstance<WorkspaceCleanUpViewModel>();

        #endregion
    }
}