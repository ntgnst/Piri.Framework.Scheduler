using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Helper;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Model;
using Piri.Framework.Scheduler.Quartz.Service;
using Quartz;

namespace Piri.Framework.Scheduler.Quartz
{
    public class Startup
    {
        private IHostingEnvironment _environment { get; }
        public IConfigurationRoot _configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            MapperInitializer.MapperConfiguration();
            services.AddDbContext<QuartzDataContext>(ServiceLifetime.Singleton);
            services.AddSingleton<IHttpHelper, HttpHelper>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<IScheduleJob, QuartzService>();

            services.UseQuartz(typeof(SimpleTestProcess));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ApplicationServices.GetService<IScheduler>();
            IScheduleJob scheduler = app.ApplicationServices.GetService<IScheduleJob>();
            scheduler.InitializeAllJobs();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc().UseMvcWithDefaultRoute();
            //Result<QuartzDto> result = Extension.QuartzServiceUtilities.StartJob<SimpleTestProcess>("0/1 * * * * ?", true).GetAwaiter().GetResult();

            app.UseMvc();
        }
    }
}
