using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using NJsonSchema.Generation;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeZoneManager.Api.Exceptions;
using TimeZoneManager.Authentication;
using TimeZoneManager.Services;

namespace TimeZoneManager.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", opt =>
                {
                    opt.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // Using JSON.NET makes update api tests to fail, apparently due to serialization incompatibilities
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //    options.JsonSerializerOptions.WriteIndented = true;
            //});

            services.AddOpenApiDocument(document =>
            {
                document.Description = "TimeZoneManager.Api";
                document.Title = "TimeZoneManager.Api";
                document.DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
                document.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token",
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Copy 'Bearer ' + valid JWT token into field",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Scheme = "Bearer",
                    }));
#pragma warning disable CS0618 // Type or member is obsolete
                document.DefaultEnumHandling = EnumHandling.String;
                document.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
#pragma warning restore CS0618 // Type or member is obsolete
            });

            services.AddTimeZoneManagerServices(Configuration);
            services.AddJwtServices();
        }

#pragma warning disable CA1822 // Mark members as static
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CA1822 // Mark members as static
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action=Index}/{id?}");
            });

            app.UseSwaggerUi3();
            app.UseOpenApi();

            app.UseApiverse(settings =>
            {
                settings.ApiverseUrl = "https://localhost:5001";
            });

            app.ApplicationServices.GetRequiredService<IDataInitializationService>().Initialize();
        }
    }
}
