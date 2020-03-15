using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using MSAL.ECommerce.ClientWeb.Models;

namespace MSAL.ECommerce.ClientWeb.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        readonly ITokenAcquisition _tokenAcquisition;
        private HttpContext _httpContext;
        static string[] _scopeRequiredByAPI = new string[] { "user.read" };

        public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeForScopes(Scopes = new[] { "user.read" })]
        public async Task<IActionResult> Privacy()
        {
            string[] scopes = new[] { "user.read" };
            //HttpContext.VerifyUserHasAnyAcceptedScope(_scopeRequiredByAPI);
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("id_token");
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var accessToken2 = HttpContext.GetTokenUsedToCallWebAPI();
                    ///await _tokenAcquisition.GetAccessTokenOnBehalfOfUserAsync(scopes, Startup.AppCfg["AzureAd:TenantId"]);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                //_tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeader(scopes, new MsalUiRequiredException("500", ex.Message));
            }
            
            return View();
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
                //await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync("AzureAD");                
                await HttpContext.SignOutAsync("AzureADCookie");

                //await HttpContext.SignOutAsync("AzureADOpenID");
            }

            return RedirectToAction("Index");
        }
    }
}
