using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Services.ClientNotifier;
using SiteMonitoring.Services.ClientOperationsDispatcher;
using SiteMonitoring.Services.IdGenerator;
using SiteMonitoring.Services.StatusChecker;
using SiteMonitoring.Services.SystemOperationsDispatcher;
using SiteMonitoring.Services.TrackingTimer;
using SiteMonitoring.Services.TrackingTimer.Quartz;
using SiteMonitoring.WebApp.SignalR;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SiteMonitoring.WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISiteStorage, InMemorySiteStorage>();
            services.AddSingleton<IIdGenerator, IdGenerator>();
            services.AddSingleton<ITrackingTimer, QuartzTrackingTimer>();
            services.AddSingleton<ITimerAction, TimerAction>();
            services.AddSingleton<IClientOperationsDispatcher, ClientOperationsDispatcher>();
            services.AddSingleton<ISystemOperationsDispatcher, SystemOperationsDispatcher>();
            services.AddSingleton<ISiteStatusChecker, SiteStatusChecker>();
            services.AddSingleton<IClientNotifier, SignalRClientNotifier>();
            services.AddTransient<UpdateSiteStatusJob>();
            services.AddHttpClient();

            services.AddSingleton<IHostedService, AvailabilityTrackerHostedService>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSignalR(routes => { routes.MapHub<MonitoringEventHub>("/monitoringEvent"); });

            app.Map("/alwaysOk", builder => builder.Run(context =>
            {
                context.Response.StatusCode = (int) HttpStatusCode.NoContent;
                return Task.CompletedTask;
            }));
            app.Map("/alwaysFail", builder => builder.Run(context =>
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return Task.CompletedTask;
            }));
            app.Map("/alwaysLong", builder => builder.Run(async context =>
            {
                await Task.Delay(5000);
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }));
            app.Map("/random", builder => builder.Run(context =>
            {
                var random = new Random();
                var isSuccess = random.Next(0, 2) == 0;
                var statusCode = isSuccess ? HttpStatusCode.NoContent : HttpStatusCode.InternalServerError;
                context.Response.StatusCode = (int)statusCode;
                return Task.CompletedTask;
            }));
        }
    }
}
