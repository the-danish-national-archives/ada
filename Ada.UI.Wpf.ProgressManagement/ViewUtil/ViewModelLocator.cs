namespace Ada.UI.Wpf.ProgressManagement.ViewUtil
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using JetBrains.Annotations;
    //using Microsoft.Practices.ServiceLocation;
    using ViewModel;

    #endregion

    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator : INotifyPropertyChanged
    {
        #region  Fields

        private readonly StringBuilder _accumErrors = new StringBuilder();

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            _accumErrors.AppendLine("Started");

            if (ServiceLocator.IsLocationProviderSet)
                SimpleIoc.Default.Reset();
            else
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

// return;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            if (ViewModelBase.IsInDesignModeStatic || !Environment.Is64BitProcess)
            {
            }

            try
            {
                // SimpleIoc.Default.Register <TableIndexViewerViewModel>();
                SimpleIoc.Default.Register<ProgressManagementViewModel>();
            }
            catch (Exception e)
            {
                _accumErrors.AppendLine(e.Message + ":" + e.InnerException?.Message);
            }

            _accumErrors.AppendLine("Constructor ended");
        }

        #endregion

        #region Properties

        public SimpleIoc IoC => SimpleIoc.Default;

        public ProgressManagementViewModel ProgressManagementViewModel
        {
            get
            {
                try
                {
                    return FacoryWithTry(
                            () =>
                                ServiceLocator.Current.GetInstance<ProgressManagementViewModel>()).Invoke()
                        ;
                }
                catch (Exception)
                {
// return _accumErrors.ToString();
                    return null;
                }
            }
        }

        public string ViewModelLocatorErrors => _accumErrors.ToString();

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        private Func<T> FacoryWithTry<T>(Func<T> f)
        {
            return () =>
            {
                _accumErrors.AppendLine($"creating {typeof(T).FullName}");
                try
                {
                    return f.Invoke();
                }
                catch (Exception e)
                {
                    _accumErrors.AppendLine(e.Message);
                    var inner = e;
                    while ((inner = inner.InnerException) != null) _accumErrors.AppendLine($"\t{inner.Message}");

                    OnPropertyChanged(nameof(ViewModelLocatorErrors));
                    throw;
                }
            };
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Func<T> ReturnSelf<T>(Func<T> f)
        {
            return f;
        }

        #endregion
    }
}