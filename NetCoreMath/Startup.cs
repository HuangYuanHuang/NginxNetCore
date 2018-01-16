using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Hangfire;
using Hangfire.Mongo;
using NetCoreMath.MathJob;
using Hangfire.Dashboard;
using Hangfire.Annotations;

namespace NetCoreMath
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
            var appConfig = Configuration.GetSection("AppConfig");
            services.Configure<AppConfig>(appConfig);
            services.AddHangfire(config =>
            {
                var storageOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        Strategy = MongoMigrationStrategy.Migrate,
                        BackupStrategy = MongoBackupStrategy.Collections
                    }
                };
                string connectString = appConfig.GetSection("MongoConfig").GetValue<string>("ConnectString");
                string dbName = appConfig.GetSection("MongoConfig").GetValue<string>("DBName");
                config.UseMongoStorage(connectString, dbName, storageOptions);
            });
            services.AddMvc();
            services.AddSingleton<IMathCoreService, MathCoreService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });


            app.UseMvc();
          
            RecurringJob.AddOrUpdate<IMathCoreService>(d => d.Excute(), Cron.MinuteInterval(1));

        }
    }

    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //var httpcontext = context.GetHttpContext();
            //return httpcontext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
