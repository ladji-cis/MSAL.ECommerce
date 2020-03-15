﻿using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensibility;
using MSAL.ECommerce.ClientDesk.Exceptions;
using MSAL.ECommerce.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace MSAL.ECommerce.ClientDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IECommerceService _eCommerceService;
        private readonly IMsGraphService _msGraphService;

        public MainWindow(IECommerceService eCommerceService, IMsGraphService msGraphService)
        {
            InitializeComponent();

            _eCommerceService = eCommerceService;
            this._msGraphService = msGraphService;
            //DG_Propducts.Visibility = Visibility.Hidden;
        }

        private async void BTN_LoadData_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                var users = await _msGraphService.GetUserInfoAsync();
                var products = await _eCommerceService.GetAllProductsAsync();
                DG_Propducts.ItemsSource = products;
                DG_Propducts.Visibility = Visibility.Visible;
            }
            catch(ApiCallException ex)
            {
                LBL_MessageInfo.Content = $"{ex.StatusCode}: {ex.Message}";
                DG_Propducts.Visibility = Visibility.Hidden;
            }            
        }

        private async void BTN_Login_Click(object sender, RoutedEventArgs e)
        {          
            if (BTN_Login.Content.Equals("LogIn"))
            {
                var scopes = new[] { "User.Read", "User.Read.All", "https://lacisorg.onmicrosoft.com/EcommerceApi/myscope" };

                try
                {
                    App.AuthenticationResult = await App.PublicClientApp.AcquireTokenSilent(scopes, await GetCurrentUser())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                    try
                    {
                        App.AuthenticationResult = await App.PublicClientApp
                                                        .AcquireTokenInteractive(scopes)
                                                        //.WithSystemWebViewOptions(new SystemWebViewOptions
                                                        //{
                                                        //    BrowserRedirectError = new Uri("")
                                                        //})
                                                        //.WithParentActivityOrWindow(new WindowInteropHelper(this).Handle)
                                                        .WithPrompt(Prompt.SelectAccount)
                                                        .ExecuteAsync();
                    }
                    catch (MsalException msalex)
                    {
                        LBL_MessageInfo.Content = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                    }

                    
                }

                IAccount account = await GetCurrentUser();
                BTN_Login.Content = $"{account?.Username} - LogOut";
            }
            else
            {
                IAccount account = await GetCurrentUser();
                await App.PublicClientApp.RemoveAsync(account);
                App.AuthenticationResult = null;
                BTN_Login.Content = "LogIn";
            }

        }

        private static async Task<IAccount> GetCurrentUser()
        {
            return (await App.PublicClientApp.GetAccountsAsync()).FirstOrDefault();
        }
    }
}
