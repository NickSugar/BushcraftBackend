using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bushcraftAPI.Filters;
using bushcraftAPI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace bushcraftAPI
{
    public class Startup
    {
        private readonly int? _httpsPort;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            // Get the HTTPS port (only in development)
            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();
                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(JsonExceptionFilter));

                //Require HTTPS for all controllers
                options.SslPort = _httpsPort;
                options.Filters.Add(typeof(RequireHttpsAttribute));

                var jsonFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                options.OutputFormatters.Remove(jsonFormatter);
                options.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Mvc.Formatters;
//using bushcraftAPI.Infrastructure;
//using Microsoft.AspNetCore.Mvc.Versioning;
//using Microsoft.AspNetCore.Mvc;
//using bushcraftAPI.Filters;

//namespace bushcraftAPI
//{
//    public class Startup
//    {
//        private readonly int? _httpsPort;

//        public Startup(IHostingEnvironment env)
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(env.ContentRootPath)
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
//                .AddEnvironmentVariables();
//            Configuration = builder.Build();

//            // Get the HTTPS port (only in development)
//            if (env.IsDevelopment())
//            {
//                var launchJsonConfig = new ConfigurationBuilder()
//                    .SetBasePath(env.ContentRootPath)
//                    .AddJsonFile("Properties\\launchSettings.json")
//                    .Build();
//                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
//            }
//        }

//        public IConfigurationRoot Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            // Add framework services.
//            services.AddMvc(opt =>
//            {
//                opt.Filters.Add(typeof(JsonExceptionFilter));

//                // Require HTTPS for all controllers
//                opt.SslPort = _httpsPort;
//                opt.Filters.Add(typeof(RequireHttpsAttribute));

//                var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
//                opt.OutputFormatters.Remove(jsonFormatter);
//                opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
//            });

//            services.AddRouting(opt => opt.LowercaseUrls = true);

//            services.AddApiVersioning(opt =>
//            {
//                opt.ApiVersionReader = new MediaTypeApiVersionReader();
//                opt.AssumeDefaultVersionWhenUnspecified = true;
//                opt.ReportApiVersions = true;
//                opt.DefaultApiVersion = new ApiVersion(1, 0);
//                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
//            });
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
//        {
//            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
//            loggerFactory.AddDebug();

//            app.UseMvc();
//            //app.UseApiVersioning();
//        }
//    }
//}

