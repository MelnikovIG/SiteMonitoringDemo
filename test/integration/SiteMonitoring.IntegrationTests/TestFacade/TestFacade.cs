using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.ClientNotifier;
using SiteMonitoring.Services.ClientOperationsDispatcher;
using SiteMonitoring.Services.IdGenerator;
using SiteMonitoring.Services.StatusChecker;
using SiteMonitoring.Services.SystemOperationsDispatcher;
using SiteMonitoring.Services.TrackingTimer;
using SiteMonitoring.Services.TrackingTimer.Quartz;
using Xunit;

namespace SiteMonitoring.IntegrationTests.TestFacade
{
    public class TestFacade
    {
        private readonly ServiceProvider _serviceProvider;

        public TestFacade()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ISiteStorage, InMemorySiteStorage>();
            serviceCollection.AddSingleton<IIdGenerator, IdGenerator>();
            serviceCollection.AddSingleton<ITrackingTimer, FakeTrackingTimer>(); 
            serviceCollection.AddSingleton<ITimerAction, TimerAction>();
            serviceCollection.AddSingleton<IClientOperationsDispatcher, ClientOperationsDispatcher>();
            serviceCollection.AddSingleton<ISystemOperationsDispatcher, SystemOperationsDispatcher>();
            serviceCollection.AddSingleton<ISiteStatusChecker, FakeSiteStatusChecker>();
            serviceCollection.AddSingleton<IClientNotifier, FakeClientNotifier>();
            serviceCollection.AddTransient<UpdateSiteStatusJob>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public async Task<SiteInfoWrapper> StartSiteMonitoring(bool siteInitiallyAvailable)
        {
            var siteUri = new Uri($"http://{Guid.NewGuid()}");
            var refreshTime = RefreshPeriod.FromSeconds(3);

            var statusChecker = (FakeSiteStatusChecker)_serviceProvider.GetService<ISiteStatusChecker>();

            if (siteInitiallyAvailable)
            {
                statusChecker.MakeAvailable(siteUri);
            }
            else
            {
                statusChecker.MakeUnavailable(siteUri);
            }

            var clientEventDispatcher = _serviceProvider.GetService<IClientOperationsDispatcher>();
            await clientEventDispatcher.StartMonitoring(new StartMonitoringModel(siteUri, refreshTime));

            var storage = _serviceProvider.GetService<ISiteStorage>();
            var createdSiteInfo = (await storage.GetSitesInfo()).Single(x => x.Uri == siteUri);

            CheckUpdateSiteInfoReceived(); //first event occured when adding site to list

            var timerAction = _serviceProvider.GetService<ITimerAction>();
            await timerAction.Execute(createdSiteInfo.Id, createdSiteInfo.Uri);

            CheckUpdateSiteInfoReceived(); //second event occured after update status timer tick

            return new SiteInfoWrapper(createdSiteInfo.Id, createdSiteInfo.Uri);
        }

        public void CheckUpdateSiteInfoReceived()
        {
            var clientNotifier = (FakeClientNotifier)_serviceProvider.GetService<IClientNotifier>();
            Assert.True(clientNotifier.ReadEvent());
        }

        public void CheckUpdateSiteInfoNotReceived()
        {
            var clientNotifier = (FakeClientNotifier)_serviceProvider.GetService<IClientNotifier>();
            Assert.False(clientNotifier.ReadEvent());
        }

        public async Task UpdateSiteMonitoring(SiteInfoWrapper site)
        {
            var siteUri = new Uri($"http://{Guid.NewGuid()}");
            var refreshTime = RefreshPeriod.FromSeconds(100);

            var clientEventDispatcher = _serviceProvider.GetService<IClientOperationsDispatcher>();
            await clientEventDispatcher.UpdateMonitoring(new UpdateMonitoringModel(site.Id, siteUri, refreshTime));

            CheckUpdateSiteInfoReceived();
        }

        public async Task RemoveSiteMonitoring(SiteInfoWrapper site)
        {
            var clientEventDispatcher = _serviceProvider.GetService<IClientOperationsDispatcher>();
            await clientEventDispatcher.EndMonitoring(new EndMonitoringModel(site.Id));

            CheckUpdateSiteInfoReceived();
        }

        public void MakeSiteAvailable(SiteInfoWrapper site)
        {
            var statusChecker = (FakeSiteStatusChecker)_serviceProvider.GetService<ISiteStatusChecker>();
            statusChecker.MakeAvailable(site.Uri);
        }

        public void MakeSiteUnavailable(SiteInfoWrapper site)
        {
            var statusChecker = (FakeSiteStatusChecker)_serviceProvider.GetService<ISiteStatusChecker>();
            statusChecker.MakeUnavailable(site.Uri);
        }

        public async Task ExecuteSiteStatusCheck(SiteInfoWrapper site)
        {
            var timerAction = _serviceProvider.GetService<ITimerAction>();
            await timerAction.Execute(site.Id, site.Uri);
        }
    }
}
