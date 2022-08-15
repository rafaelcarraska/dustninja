using System.Collections.Generic;
using System.Globalization;
using DustMedicalNinja.Context;
using DustMedicalNinja.Policies;
using DustMedicalNinja.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DustMedicalNinja
{
    public class Startup
    {
        internal static string stringConexaoMongo;
        internal static string stringConexaoPostgre;
        internal static string ambiente;
        internal static string urlFont;

        internal static string path;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ambiente = env.EnvironmentName;
            path = env.ContentRootPath;

            if (env.IsDevelopment())
            {
                //stringConexaoMongo = Configuration.GetConnectionString("Conection_mongo_dev");
                //stringConexaoPostgre = Configuration.GetConnectionString("Conection_Postgre_dev");
                //urlFont = "http://localhost:4200";

                app.UseHsts();
                stringConexaoMongo = Configuration.GetConnectionString("Conection_mongo_prod");
                stringConexaoPostgre = Configuration.GetConnectionString("Conection_Postgre_prod");
                urlFont = "https://fastpacs.com.br";
            }
            else 
            {   
                app.UseHsts();
                stringConexaoMongo = Configuration.GetConnectionString("Conection_mongo_local");
                stringConexaoPostgre = Configuration.GetConnectionString("Conection_Postgre_prod");
                urlFont = "https://fastpacs.com.br";

                if (env.IsEnvironment("UAT"))
                {
                    stringConexaoPostgre = Configuration.GetConnectionString("Conection_Postgre_uat");
                    urlFont = "https://uat.fastpacs.com.br";
                }
                if (env.IsEnvironment("DEV"))
                {
                    stringConexaoPostgre = Configuration.GetConnectionString("Conection_Postgre_dev");
                    urlFont = "https://health.dustmedical.ninja";
                }

            }

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowCredentials().AllowAnyHeader().AllowAnyMethod());

            //app.UseCors(builder =>
            //    builder.WithOrigins("https://health.dustmedical.ninja")
            //           .AllowAnyHeader()
            //    );

            //app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthentication();

            CultureInfo cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvcWithDefaultRoute();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddMvc(options => options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())))
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSingleton<IAuthorizationHandler, OnlyExpensiveMastreAuthorizationHandler>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            void JwtBearer(JwtBearerOptions jwtBearer)
            {
                jwtBearer.TokenValidationParameters = new JsonWebToken().TokenValidationParameters;
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearer);

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    List<CultureInfo> supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-US")
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddEntityFrameworkNpgsql()
            .AddDbContext<DCMContext>(options => options.UseNpgsql(stringConexaoPostgre));
        }

    }
}
