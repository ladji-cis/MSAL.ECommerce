using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSAL.ECommerce.Shared.Services;
using MSAL.ECommerce.Shared;
using Polly;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Identity.Client;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MSAL.ECommerce.Shared.Services;

namespace MSAL.ECommerce.ClientDesk
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public static IPublicClientApplication PublicClientApp;

        public static AuthenticationResult AuthenticationResult;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            Configuration = builder.Build();

            PublicClientApp = PublicClientApplicationBuilder.Create(Configuration["AzureAd:ClientId"])
                .WithAuthority(AzureCloudInstance.AzurePublic, Configuration["AzureAd:Domain"])
                .WithDebugLoggingCallback(LogLevel.Verbose)     
                .WithRedirectUri(Configuration["AzureAd:RedirectUri"])
                .Build();

            TokenCacheHelper.EnableSerialization(PublicClientApp.UserTokenCache);;

            var serviceCollection = new ServiceCollection();
            
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(MainWindow));

            services.AddHttpClient();

            services.AddHttpClient<IECommerceService, ECommerceService>(cfg =>
            {
                cfg.BaseAddress = new Uri(Configuration["AppSettings:ECommerceApiUrl"]);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, () => TimeSpan.FromSeconds(5)));

            services.AddHttpClient<IMsGraphService, MsGraphService>(cfg =>
            {
                cfg.BaseAddress = new Uri(Configuration["AppSettings:MsGraphApiUrl"]);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));
        }
    }
}
