using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalaryCounter.Api.Middleware;
using SalaryCounter.Api.Swagger;
using SalaryCounter.Api.Util;
using SalaryCounter.Dao.Extension;
using SalaryCounter.Model.Extension;
using SalaryCounter.Service.Extension;
using SalaryCounter.Service.Util;

namespace SalaryCounter.Api
{
    internal class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvcCore()
                .AddNewtonsoftJson(options => options.SerializerSettings.Configure());
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddVersionedApiExplorer(options =>
            {
                // ReSharper disable once StringLiteralTypo
                options.GroupNameFormat = "VVVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddOpenApiDocument((document, serviceProvider) =>
            {
                document.SchemaProcessors.Add(new FilterModelProcessor());
                document.SchemaProcessors.Add(new ModelExampleProcessor(serviceProvider));
                document.DocumentName = "v1.0";
                document.ApiGroupNames = new[] {"1.0"};
            });
            var container = new DependencyInjectionContainer(services);
            container.ConfigureService();
            container.ConfigureDao();
            container.AddSingleton<IAppConfiguration, AppConfiguration>();
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction()) app.UseDeveloperExceptionPage();
            if (env.IsProduction()) app.UseHsts();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseOpenApi();
            app.UseSwaggerUi3(config => config.DocumentTitle = "Salary counter");
            if (!env.IsEnvironment("NoSpa"))
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.Options.StartupTimeout = TimeSpan.FromSeconds(120);

                    if (env.IsDevelopment()) spa.UseAngularCliServer("start");
                });
        }
    }
}