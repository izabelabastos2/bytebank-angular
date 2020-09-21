using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using Vale.Geographic.Api.Filters;
using Vale.Geographic.Api.Policies;
using Vale.Geographic.Api.Provider;
using Vale.Geographic.Infra.CrossCutting.IoC;

namespace Vale.Geographic.Api
{
    public class Startup
    {
        public static readonly List<string> AuthenticatedEnvironments = new List<string>() { "production", "qa", "local", "development" };

        private readonly SimpleInjectorBootStrapper Injector = new SimpleInjectorBootStrapper();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       // public static ImmutableArray<string> AuthenticatedEnvironments { get => authenticatedEnvironments; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var options = app.ApplicationServices
                .GetService<IOptions<JwtIssuerOptions>>()
                .Value;


            Injector.InitializeContainer(app, Configuration);
            Injector.Verify();


            loggerFactory.AddAzureWebAppDiagnostics(
                new AzureAppServicesDiagnosticsSettings
                {
                    OutputTemplate =
                        "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}"
                }
            );


            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();


            app.UseAuthentication();

            app.UseCors("ValePolicy");
            app.UseMiddleware(typeof(InterceptorHandlingMiddleware));
            app.UseResponseCompression();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Vale.Geographic API V1 - " + env.EnvironmentName);
                c.SwaggerEndpoint("v2/swagger.json", "Vale.Geographic API V2 - " + env.EnvironmentName);
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            services.AddApiVersioning(options =>
            {
                options.UseApiBehavior = false;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("x-api-version"),
                    new QueryStringApiVersionReader(),
                    new UrlSegmentApiVersionReader());
            });

            services.AddVersionedApiExplorer(
              options =>
              {
                  options.GroupNameFormat = "'v'VVV";
                  options.SubstituteApiVersionInUrl = true;
              });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml" });
            });            

            services.AddCors(options =>
            {
                if (!AuthenticatedEnvironments.Contains(environment))
                {

                    options.AddPolicy("ValePolicy",
                        builder =>
                        {
                            builder
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowAnyOrigin()
                                .WithExposedHeaders("X-Total-Count");
                        });
                }
                else
                {
                    options.AddPolicy("ValePolicy",
                        builder =>
                        {
                            builder
                                .WithExposedHeaders("X-Total-Count");
                        });
                }

            });

            services.ConfigureJwtServices(Configuration);
            Injector.ConfigureEntityFramework(services, Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = $"Vale.Geographic API ({environment})",
                        Description = "List of microservice to manage the entities",
                        Version = "v1",

                        Contact = new Contact
                        {
                            Name = "Digital Solutions",
                            Email = "C0497842@vale.com"
                        }
                    });

                c.SwaggerDoc("v2",
                   new Info
                   {
                       Title = $"Vale.Geographic API ({environment}) - v2.0",
                       Description = "List of microservice to manage the entities",
                       Version = "v2",

                       Contact = new Contact
                       {
                           Name = "Digital Solutions",
                           Email = "C0497842@vale.com"
                       }
                   });

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter JWT with Bearer into field",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }}
                });


                c.DescribeAllEnumsAsStrings();
                c.OrderActionsBy(apiDesc => apiDesc.HttpMethod.ToString());
                c.CustomSchemaIds(x => x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("ValePolicy"));
            });

            var tokenAuthorizeIntegrationWs = Configuration.GetSection("AuthorizeApp")["IntegrationWs"];

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IntegrationWs", policy =>
                    policy.Requirements.Add(new AuthorizeAppRequirement(tokenAuthorizeIntegrationWs, environment)));
            });

            services.AddSingleton<IAuthorizationHandler, AuthorizeAppHandler>();
            var mvc = services.AddMvc(setupAction =>
            {
                if (!AuthenticatedEnvironments.Contains(environment))
                    setupAction.Filters.Add(new AllowAnonymousFilter());

                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            mvc.AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                });
            Injector.IntegrateSimpleInjector(services);
        }
    }
}