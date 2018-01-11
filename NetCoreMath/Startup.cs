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

                 config.UseMongoStorage("mongodb://hyhrobot.com:27017", "Hangfire", storageOptions);

            });
            services.AddMvc();
            services.AddSingleton<IMathCoreService, MathCoreService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // GlobalConfiguration.Configuration.UseMongoStorage("mongodb://192.168.219.129:27017/?connectTimeoutMS=30000&maxIdleTimeMS=600000", "webrtc");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireServer();
            app.UseHangfireDashboard();


            app.UseMvc();
            RecurringJob.AddOrUpdate<IMathCoreService>(d => d.Excute(), Cron.MinuteInterval(1));
            
        }
    }
}
