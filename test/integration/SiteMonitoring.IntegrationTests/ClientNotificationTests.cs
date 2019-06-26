using System.Threading.Tasks;
using Xunit;

namespace SiteMonitoring.IntegrationTests
{
    public class ClientNotificationTests
    {
        private readonly TestFacade.TestFacade _facade;

        public ClientNotificationTests()
        {
            _facade = new TestFacade.TestFacade();
        }

        [Fact]
        public async Task When_MonitoringAdded_Clients_ShouldBeNotified()
        {
            await _facade.StartSiteMonitoring(true);
        }

        [Fact]
        public async Task When_MonitoringEdited_Clients_ShouldBeNotified()
        {
            var site = await _facade.StartSiteMonitoring(true);

            await _facade.UpdateSiteMonitoring(site);
        }

        [Fact]
        public async Task When_MonitoringRemoved_Clients_ShouldBeNotified()
        {
            var site = await _facade.StartSiteMonitoring(true);

            await _facade.RemoveSiteMonitoring(site);
        }

        [Fact]
        public async Task When_SiteStatus_Changed_FromOnlineToOffline_ClientShouldBeNotified()
        {
            var site = await _facade.StartSiteMonitoring(true);
            _facade.MakeSiteUnavailable(site);
            await _facade.ExecuteSiteStatusCheck(site);
            _facade.CheckUpdateSiteInfoReceived();
        }

        [Fact]
        public async Task When_SiteStatus_Changed_FromOfflineToOnline_ClientShouldBeNotified()
        {
            var site = await _facade.StartSiteMonitoring(false);
            _facade.MakeSiteAvailable(site);
            await _facade.ExecuteSiteStatusCheck(site);
            _facade.CheckUpdateSiteInfoReceived();
        }

        [Fact]
        public async Task When_SiteStatus_NotChanging_ClientShouldNotBeNotified()
        {
            var site = await _facade.StartSiteMonitoring(true);
            await _facade.ExecuteSiteStatusCheck(site);
            _facade.CheckUpdateSiteInfoNotReceived();
        }
    }
}
