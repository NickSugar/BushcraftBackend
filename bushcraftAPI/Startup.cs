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
using bushcraftAPI.Models;
using Microsoft.EntityFrameworkCore;

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
            //Use local database for dev and testing
            //TODO: Switch to real database in production
            services.AddDbContext<BushcraftApiContext>(options => options.UseInMemoryDatabase("MyDB"));

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

            services.Configure<GameInfo>(Configuration.GetSection("GameInfo"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Test data
                var context = app.ApplicationServices.GetRequiredService<BushcraftApiContext>();
                AddTestData(context);
            }

            app.UseHsts(options =>
            {
                options.MaxAge(days: 180);
                options.IncludeSubdomains();
                options.Preload();
            });

            app.UseMvc();
        }

        private static void AddTestData(BushcraftApiContext context)
        {
            context.Gear.Add(new GearEntity
            {
                Id = Guid.Parse("6e00b034-f52f-4492-a4e6-6358b7e314e6"),
                Name = "Axe, 19\" Wooden Carpenter's",
                Description = "Husqvarna provides a wide range of wooden axes for different kinds of work. These axes are forged in Sweden from Swedish axe steel with a consistently high quality. With good maintenance, your axe will last for a long time. Don't store in too warm conditions, since the handle might Shrink. Always dry of dirt & moisten before putting the axe cover on. If the axe is put away for a longer time, grease it to prevent rust.",
                Price = 5421,
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/71x6AwqeCpL._SL1500_.jpg",
                Url = "https://www.amazon.com/gp/product/B004SN1HGQ/?tag=outdoorsmag-20",
                HitPoints = 10000,
                Weight = 1100,
                Height = 165,
                Length = 508,
                Width = 41,
                Volume = 1000,
                StorageVolume = 0
            });

            context.SaveChanges();
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

