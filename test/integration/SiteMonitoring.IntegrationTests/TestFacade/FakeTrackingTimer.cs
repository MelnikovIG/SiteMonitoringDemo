using System;
using System.Threading.Tasks;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.TrackingTimer;

namespace SiteMonitoring.IntegrationTests.TestFacade
{
    /// <summary>
    /// Nothing to to there, we are manually controlling ticks for tests
    /// </summary>
    public class FakeTrackingTimer : ITrackingTimer
    {
        public Task Start(Guid siteId, Uri uri, RefreshPeriod refreshPeriod)
        {
            return Task.CompletedTask;
        }

        public Task Update(Guid siteId, Uri uri, RefreshPeriod refreshPeriod)
        {
            return Task.CompletedTask;
        }

        public Task Stop(Guid siteId)
        {
            return Task.CompletedTask;
        }
    }
}