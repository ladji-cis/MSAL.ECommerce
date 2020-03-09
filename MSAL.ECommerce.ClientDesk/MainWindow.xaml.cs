using MSAL.ECommerce.ClientDesk.Exceptions;
using MSAL.ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSAL.ECommerce.ClientDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IECommerceService _eCommerceService;

        public MainWindow(IECommerceService eCommerceService)
        {
            InitializeComponent();

            _eCommerceService = eCommerceService;
            //DG_Propducts.Visibility = Visibility.Hidden;
        }

        private async void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
    }
}
