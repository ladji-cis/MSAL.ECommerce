using System;
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
using MSAL.ECommerce.Shared;
using Newtonsoft.Json;

namespace MSAL.ECommerce.ClientWeb.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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
                var userInfo = await GetUserInfoAsync(accessToken);
                ViewBag.UserInfo = userInfo;
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            
            return View();
        }

        private async Task<UserInfo> GetUserInfoAsync(string accessToken)
        {
            var httpClient = new HttpClient();

            var graphApiUrl = "https://graph.microsoft.com/v1.0";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{graphApiUrl}/me");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(content);
                return userInfo;
            }

            return null;
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
