﻿using System;
using Liquid.Middleware;
using Liquid.OnAzure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Liquid.Sample.CarRegistry
{
    /// <summary>
    /// Startup Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup Constructor
        /// </summary>
        /// <param name="configuration">Object IConfiguration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Object IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Object IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Add the configuration on WorkBench
            services.AddWorkBench(Configuration);

            //services.AddDistributedRedisCache(options =>
            //{
            //    options.InstanceName = Configuration.GetSection( "Redis:Name").Value;
            //    options.Configuration = Configuration.GetSection("Redis:Host").Value;
            //});
            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Start the configuration on WorkBench
            WorkBench.UseTelemetry<AppInsights>();
            WorkBench.UseRepository<CosmosDB>();
            WorkBench.UseMessageBus<ServiceBus>();
            WorkBench.UseCache<AzureRedis>();
            app.UseWorkBench();
            
            app.UseMvc();
        }
    }
}
