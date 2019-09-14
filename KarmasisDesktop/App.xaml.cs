using ElasticWCFDataAccess;
using KarmasisDesktop.Views;
using KarmasisDesktopCore;
using KarmasisDomain.Repositories;
using KarmasisPresentation.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KarmasisDesktop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainUnhandledException);

                IUnityContainer container = new UnityContainer();
                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
                container.RegisterInstance<ITimer>(new Timer());
                container.RegisterInstance<IDispatcher>(new DispatcherWrapper());
               
                var ticketRepository = new DataServiceWrapper(ConfigurationManager.ConnectionStrings["ElasticWCFSettings"].ConnectionString);
                container.RegisterInstance<TicketRepository>(ticketRepository);
              
                //var ticketRepository = new ElasticWCFDataAccess.SecureDataServiceWrapper(ConfigurationManager.ConnectionStrings["ElasticWCFSettings"].ConnectionString);
                //container.RegisterInstance<TicketRepository>(ticketRepository);
                var window = container.Resolve<MainWindow>();
                window.Show();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            MessageBox.Show(
                ex.Message,
                ex.Message,
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );

            Environment.Exit(1);
        }
    }
}
