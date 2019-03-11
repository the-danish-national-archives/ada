namespace Ada.Test.UI.Wpf.TableIndex.ViewModel
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Ada.UI.Wpf.ViewModel;
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    //using Microsoft.Practices.ServiceLocation;
    using Ra.Common.Repository.NHibernate;
    using Ra.DomainEntities;
    using Ra.DomainEntities.TableIndex;
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


            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


            if (ViewModelBase.IsInDesignModeStatic || !Environment.Is64BitProcess)
            {
                //                // Code runs in Blend --> create design time data.
                //                SimpleIoc.Default.Register<TableIndex>(() =>
                //                {
                //                    XmlEventFilter filter = new XmlEventFilter();
                //                    IArchivalXmlReader reader = new ArchivalXmlReader(filter);
                //                    
                //
                //                    var xmlStream = ResourceUtil.GetEmbeddedResourceStreamFromPath(@"SampleAVID\Indices\tableIndex.xml");
                //                   
                //                    var xsdStream = ResourceUtil.GetEmbeddedResourceStreamFromPath(@"SampleAVID\Schemas\Standard\tableIndex.xsd");
                //
                //
                //                    return TableIndexloader.Load(reader, new BufferedProgressStream(xmlStream),
                //                        new BufferedProgressStream(xsdStream));
                //                });
                //                SimpleIoc.Default.Register<IDbConnection>(() =>
                //                {
                //                    return null;
                //                });
//                new Ada.UI.Wpf.TableIndexViewer.ViewUtil.ViewModelLocator();
            }
            else
            {
                var defaultTestDataset = "10001";
                Func<string, TableIndex> TableIndexGenerator = name =>
                {
                    var factory = new AdaTestUowFactory(new AViD("SA", name), "test", new DirectoryInfo(@"..\..\SampleAVID"));

                    return factory.GetUnitOfWork().GetRepository<TableIndex>().All().FirstOrDefault();
                };
                SimpleIoc.Default.Register<Func<string, TableIndex>>(() => TableIndexGenerator);
                SimpleIoc.Default.Register<TableIndex>(() => TableIndexGenerator(defaultTestDataset));
                Func<string, IDbConnection> IDbConnectionGenerator = name =>
                {
                    var factory = new AdaAvUowFactory(new AViD("SA", name), "av", new DirectoryInfo(@"..\..\SampleAVID"));

                    return ((UnitOfWork) factory.GetUnitOfWork()).Session.Connection;
                };
                SimpleIoc.Default.Register<Func<string, IDbConnection>>(() => IDbConnectionGenerator);
                SimpleIoc.Default.Register<IDbConnection>(() => IDbConnectionGenerator(defaultTestDataset));
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        #endregion

        #region Properties

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        #endregion
    }
}