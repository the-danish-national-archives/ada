/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Ada.Test.UI.Wpf.ProgressManagement"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

namespace Ada.Test.UI.Wpf.ProgressManagement.ViewModel
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using Action64.IngestActions;
    using ActionBase;
    using Actions;
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using JetBrains.Annotations;
    using Log;
    //using Microsoft.Practices.ServiceLocation;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;
    using Repositories;
    using WPFLocalizeExtension.Engine;

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
            if (ServiceLocator.IsLocationProviderSet)
                return;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("da-dk");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture; // Thread.CurrentThread.CurrentCulture.;
            LocalizeDictionary.Instance.Culture = Thread.CurrentThread.CurrentCulture;

            AdaActionContainer.Instance.Load<DocumentsOtherIngestAction>();
            AdaActionContainer.Instance.Load<AdaSingleQueryAction>();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic || !Environment.Is64BitProcess)
            {
            }
            else
            {
                var defaultTestDataset = "17188";
//                Func<string, TableIndex> TableIndexGenerator = (name) =>
//                {
//                    var factory = new AdaTestUowFactory(new AViD("SA", name), "test", new DirectoryInfo(@"..\..\SampleAVID"));
//
//                    return factory.GetUnitOfWork().GetRepository<TableIndex>().All().FirstOrDefault();
//                };
//                SimpleIoc.Default.Register<Func<string, TableIndex>>(() => TableIndexGenerator);
//                SimpleIoc.Default.Register<TableIndex>(() => TableIndexGenerator(defaultTestDataset));
                Func<string, IDbConnection> IDbConnectionGenerator = name =>
                {
                    var factory = new AdaAvUowFactory(new AViD("SA", name), "test", new DirectoryInfo(@"..\..\SampleAvIds"));

                    return ((UnitOfWork) factory.GetUnitOfWork()).Session.Connection;
                };
                SimpleIoc.Default.Register<Func<string, IDbConnection>>(() => IDbConnectionGenerator);
                SimpleIoc.Default.Register<IDbConnection>(() => IDbConnectionGenerator(defaultTestDataset));

                Func<string, AdaTestLog> AdaTestLogGenerator = name =>
                {
                    var logFactory = new AdaLogUowFactory(new AViD("SA", name), "log", new DirectoryInfo(@"..\..\SampleAvIds"));


                    return new AdaTestLog(logFactory, Guid.Empty, GetType(), 0);
                };
                SimpleIoc.Default.Register<Func<string, AdaTestLog>>(() => AdaTestLogGenerator);
                SimpleIoc.Default.Register<AdaTestLog>(() => AdaTestLogGenerator(defaultTestDataset));
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        #endregion

        #region Properties

        [NotNull]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        #endregion

        #region

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        #endregion
    }
}