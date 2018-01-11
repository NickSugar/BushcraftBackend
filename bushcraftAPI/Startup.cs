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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => {
                options.Filters.Add(typeof(JsonExceptionFilter));

                var jsonFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                options.OutputFormatters.Remove(jsonFormatter);
                options.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddApiVersioning(options => {
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
