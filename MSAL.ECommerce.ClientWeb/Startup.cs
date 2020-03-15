using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
//using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.IdentityModel.Logging;

namespace MSAL.ECommerce.ClientWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;

            Configuration = configuration;
            AppCfg = configuration;
        }

        public IConfiguration Configuration { get; }

        public static IConfiguration AppCfg;

        public static string[] Scopes = new string[] { "User.Read", "https://lacisorg.onmicrosoft.com/EcommerceApi/myscope" };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddControllersWithViews(options =>
            {
                var authPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(authPolicy));
            });

            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //    .AddAzureAD(options => Configuration.Bind("AzureAd"));

            services.AddMicrosoftIdentityPlatformAuthentication(Configuration, "AzureAd", true)
                      .AddMsal(Configuration, Scopes)
                      .AddInMemoryTokenCaches();

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                //options.Authority = options.Authority + "/v2.0/";
                options.SaveTokens = true;

                //A list of parameters for token validation. 
                //In this case, ValidateIssuer is set to false to indicate that it can accept sign-ins from any personal, or work or school accounts.
                //options.TokenValidationParameters.ValidateIssuer = false;

                options.Events.OnAuthenticationFailed = (ctx) => 
                { 
                    return Task.FromResult(0); 
                };

                options.Events.OnAuthorizationCodeReceived = (ctx) =>
                {
                    return Task.FromResult(0);
                };

                options.Events.OnMessageReceived = (ctx) =>
                {
                    return Task.FromResult(0);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
