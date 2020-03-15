using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MSAL.ECommerce.Api.Storage;

namespace MSAL.ECommerce.Api
{
    public class Startup
    {
        // Secret
        //.bMsx8atIx@UiiVZWX6r4gRqG6/]2bq_

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));


            //services.AddProtectedWebApi(Configuration)
            //        .AddInMemoryTokenCaches();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"{Configuration["AzureAd:Instance"]}/{Configuration["AzureAd:TenantId"]}/v2.0/";
                    options.Audience = Configuration["AzureAd:Audience"];
                    options.ClaimsIssuer = $"{Configuration["AzureAd:Issuer"]}/{Configuration["AzureAd:TenantId"]}";
                    options.TokenValidationParameters.ValidateIssuer = false;
                });

            //services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            //{
            //    options.Authority = $"{options.Authority}/v2.0/";
            //    options.ClaimsIssuer = $"{Configuration["AzureAd:Issuer"]}/{Configuration["AzureAd:TenantId"]}";

            //    options.TokenValidationParameters.ValidateAudience = false;
            //    options.TokenValidationParameters.ValidateIssuer = false;
            //});

            services.AddScoped<ICatalogService, CatalogService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSAL.ECommerce Api", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "MSAL.ECommerce.Api.xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();            

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSAL.ECommerce Api V1");
            });            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
