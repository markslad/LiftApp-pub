using System;
using System.Windows;
using LiftApp.ViewModels;
using LiftApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LiftApp.Dal.Extensions;
using LiftApp.Extensions;
using LiftApp.Options;
using LiftApp.Export.Extensions;
using NLog.Web;
using LiftApp.Statistics.Extensions;

namespace LiftApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        private readonly ILogger<App> _logger;

        public App()
        {
            _host = CreateHostBuilder().Build();
            _logger = _host.Services.GetRequiredService<ILogger<App>>();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
            _logger.LogInformation("Application started");
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            var mainWindowViewModel = _host.Services.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
                _logger.LogInformation("Application stopped");
            }
        }

        private static IHostBuilder CreateHostBuilder()
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(AppContext.BaseDirectory);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddNLog("nlog.config");
                });

            ConfigureServices(builder);

            return builder;
        }

        private static void ConfigureServices(IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.RegisterDataAccessLayerServices(context.Configuration);
                services.RegisterExportServices(context.Configuration);
                services.RegisterReportServices(context.Configuration);
                services.RegisterViews(context.Configuration);
                services.RegisterViewModels(context.Configuration);

                services.Configure<NewInvoiceOptions>(context.Configuration.GetSection(nameof(NewInvoiceOptions)));
            });
        }
    }
}
