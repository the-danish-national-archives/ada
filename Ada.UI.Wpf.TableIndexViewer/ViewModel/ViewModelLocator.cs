#define VIEW

namespace Ada.UI.Wpf.TableIndexViewer.ViewUtil
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using CommonServiceLocator;
    using DataTableViewer.ViewModel;
    using EntityLoaders;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    //using Microsoft.Practices.ServiceLocation;
    using Ra.Common;
    using Ra.Common.Repository.NHibernate;
    using Ra.Common.Wpf;
    using Ra.Common.Xml;
    using Ra.DomainEntities;
    using Ra.DomainEntities.TableIndex;
    using Repositories;
    using ViewModel;

    #endregion

    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        #region  Fields

        private readonly StringBuilder _accumErrors = new StringBuilder();

        #endregion

        #region  Constructors

        // Func<Func<T>,string,Func<T>> FacoryWithTry<T> = (f,t) =>
        // {
        // return () =>
        // {
        // try
        // {
        // } catch (Exception e)
        // {
        // throw e;
        // }
        // }
        // }

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
// Code runs in Blend --> create design time data.
                SimpleIoc.Default.Register(
                    FacoryWithTry(
                        () =>
                        {
                            var filter = new XmlEventFilter();
                            IArchivalXmlReader reader = new ArchivalXmlReader(filter);

                            var xmlStream =
                                ResourceUtil.GetEmbeddedResourceStreamFromPath<ViewModelLocator>(
                                    @"SampleAVID\Indices\tableIndex.xml");

                            var xsdStream =
                                ResourceUtil.GetEmbeddedResourceStreamFromPath<ViewModelLocator>(
                                    @"SampleAVID\Schemas\Standard\tableIndex.xsd");

                            return TableIndexloader.Load(
                                reader,
                                new BufferedProgressStream(xmlStream),
                                new BufferedProgressStream(xsdStream));

// return null;
                        }));
                SimpleIoc.Default.Register(
                    FacoryWithTry<IDbConnection>(() => { return null; }));
            }
            else
            {
                var defaultTestDataset = "10001";
                Func<string, TableIndex> tableIndexGenerator = name =>
                {
                    _accumErrors.AppendLine($"{name} TableIndex requested");

                    IAdaUowFactory testFactory =
                        new AdaTestUowFactory(new AViD("SA", name), "test", new DirectoryInfo(@"..\..\SampleAVID"));

                    var uowTest = testFactory.GetUnitOfWork();

                    var repository = uowTest.GetRepository<TableIndex>();
                    var tableIndex = repository.All().FirstOrDefault();

                    return tableIndex;
                };
                SimpleIoc.Default.Register(() => tableIndexGenerator);
                SimpleIoc.Default.Register(() => tableIndexGenerator(defaultTestDataset));
                Func<string, IDbConnection> dbConnectionGenerator = name =>
                {
                    _accumErrors.AppendLine($"{name} IDbConnection requested");

                    IAdaUowFactory AVFactory = new AdaAvUowFactory(
                        new AViD("SA", name),
                        "av",
                        new DirectoryInfo(@"..\..\SampleAVID"));
                    var uowAv = (UnitOfWork) AVFactory.GetUnitOfWork();
                    return uowAv.Session.Connection;
                };
                SimpleIoc.Default.Register(() => dbConnectionGenerator);
                SimpleIoc.Default.Register(() => dbConnectionGenerator(defaultTestDataset));
            }

// #if
            try
            {
                SimpleIoc.Default.Register<TableIndexViewerViewModel>();
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

        public IQueryViewModel QueryViewModelView => ServiceLocator.Current.GetInstance<QueryViewModelView>();

        public DataTableViewModel SqlShowViewModel => ServiceLocator.Current.GetInstance<DataTableViewModel>();

        public TableIndexViewerViewModel TableIndexViewerViewModel
        {
            get
            {
                try
                {
                    return FacoryWithTry(
                            () =>
                                ServiceLocator.Current.GetInstance<TableIndexViewerViewModel>()).Invoke()
                        ;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        #region

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

                    throw;
                }
            };
        }

        #endregion
    }
}