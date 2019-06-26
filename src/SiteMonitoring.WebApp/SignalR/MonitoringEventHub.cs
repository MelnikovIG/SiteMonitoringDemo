using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.ClientOperationsDispatcher;

namespace SiteMonitoring.WebApp.SignalR
{
    public class MonitoringEventHub : Hub<IMonitoringClient>
    {
        private readonly ISiteStorage _siteStorage;
        private readonly IClientOperationsDispatcher _availabilityTracker;

        public MonitoringEventHub(ISiteStorage siteStorage, IClientOperationsDispatcher availabilityTracker)
        {
            _siteStorage = siteStorage;
            _availabilityTracker = availabilityTracker;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var data = await _siteStorage.GetSitesInfo();

            var processedData = data.Select(x =>
                    new SiteInfoClientData(
                        x.Id,
                        x.Uri.ToString(),
                        x.RefreshTimeInSeconds.Seconds,
                        (int) x.Status))
                .ToArray();

            await Clients.Caller.ReceiveMonitoringData(processedData);
        }

        public async Task RemoveRecord(Guid id)
        {
            await _availabilityTracker.EndMonitoring(new EndMonitoringModel(id));
        }

        public async Task CreateRecord(string uri, ulong refreshTme)
        {
            await _availabilityTracker.StartMonitoring(new StartMonitoringModel(new Uri(uri), RefreshPeriod.FromSeconds(refreshTme)));
        }

        public async Task UpdateRecord(Guid id,string uri, ulong refreshTme)
        {
            await _availabilityTracker.UpdateMonitoring(new UpdateMonitoringModel(id, new Uri(uri), RefreshPeriod.FromSeconds(refreshTme) ));
        }
    }
}
