using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SCRUM.Helpers;
using SCRUM.Helpers.Authorization;
using SCRUM.Models;
using static SCRUM.Helpers.XpoSerialization;

namespace SCRUM
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                        builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("https://localhost:44358")
                        .AllowCredentials());
            });
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            #region Authentication and Authorization

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // Sets the default scheme to cookies
                .AddCookie(options => options.LoginPath = "/api/signin");

            services.AddAuthorization();
            #endregion

            #region AutoMapper

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            #endregion

            services
                .AddXpoDefaultUnitOfWork(true, options => options
                    .UseConnectionString(Configuration.GetConnectionString("InMemoryDataStore"))
                    .UseThreadSafeDataLayer(true)
                    .UseConnectionPool(false) // Remove this line if you use a database server like SQL Server, Oracle, PostgreSql, etc.                    
                    .UseAutoCreationOption(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema)// Remove this line if the database already exists
                    .UseEntityTypes(
                        typeof(Requirement), 
                        typeof(PendingRequirement), 
                        typeof(Project), 
                        typeof(ProjectUser),
                        typeof(Role),
                        typeof(RoleAccess)
                    )// Pass all of your persistent object types to this method.
                );

            services.AddHttpContextAccessor();
            services.ConfigureOptions<ConfigureJsonOptions>();
            services.AddSingleton(typeof(IModelMetadataProvider), typeof(XpoMetadataProvider));
            services.AddScoped<IAuthorizationHandler, DataAuthorizationHandler>();
            services.AddTransient<InitializeData>();
            services.AddSingleton(mapper);
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, InitializeData initializeData)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            #region Initialize data
            
            initializeData.Initialize().Wait();

            #endregion
        }
    }
}
