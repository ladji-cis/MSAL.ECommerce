using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using MSAL.ECommerce.ClientWeb.Models;
using MSAL.ECommerce.Shared.Models;
using MSAL.ECommerce.Shared.Services;
using Newtonsoft.Json;

namespace MSAL.ECommerce.ClientWeb.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IECommerceService _eCommerceService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IECommerceService eCommerceService, ILogger<HomeController> logger)
        {
            _eCommerceService = eCommerceService;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeForScopes(Scopes = new[] { "user.read" })]
        public async Task<IActionResult> Privacy()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                // Graph API
                var userInfo = await GetAdUsersAsync(accessToken);
                ViewBag.UserInfos = userInfo;

                // Ecommerce API
                var products = await GetProductsAsync(accessToken);
                ViewBag.Products = products;
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            
            return View();
        }

        private async Task<IEnumerable<UserInfo>> GetAdUsersAsync(string accessToken)
        {
            var httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{Startup.AppCfg["AppSettings:MsGraphApiUrl"]}/users");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var userInfos = JsonConvert.DeserializeObject<UserInfoMetadata>(content).Value;
                return userInfos;
            }

            return null;
        }

        private async Task<IEnumerable<Product>> GetProductsAsync(string accessToken)
        {
            var products = await _eCommerceService.GetAllProductsAsync(accessToken);
            return products;
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignIn()
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SignOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync("AzureAD");                
                await HttpContext.SignOutAsync("AzureADCookie");
            }

            return RedirectToAction("Index");
        }
    }
}
