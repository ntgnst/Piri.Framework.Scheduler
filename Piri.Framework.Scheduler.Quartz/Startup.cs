using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piri.Framework.Scheduler.Quartz.Extension;
using Quartz;

namespace Piri.Framework.Scheduler.Quartz
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
            services.AddMvc();

            services.UseQuartz(typeof(SimpleTestProcess));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ApplicationServices.GetService<IScheduler>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Equals("/StartJobs"))
                {
                    //Business Logic
                    QuartzServiceUtilities.StartJob<SimpleTestProcess>("0/5 * * * * ?");
                }
                
            });

            app.UseMvc();
        }
    }
}
