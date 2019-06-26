using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Services.ClientNotifier;

namespace SiteMonitoring.WebApp.SignalR
{
    /// <summary>
    /// Client notifier using SignalR
    /// </summary>
    public class SignalRClientNotifier : IClientNotifier
    {
        private readonly IHubContext<MonitoringEventHub, IMonitoringClient> _hubContext;
        private readonly ISiteStorage _siteStorage;

        public SignalRClientNotifier(IHubContext<MonitoringEventHub, IMonitoringClient> hubContext, ISiteStorage siteStorage)
        {
            _hubContext = hubContext;
            _siteStorage = siteStorage;
        }

        public async Task UpdateSitesInfo()
        {
            var data = await _siteStorage.GetSitesInfo();

            var processedData = data.Select(x =>
                    new SiteInfoClientData(
                        x.Id,
                        x.Uri.ToString(),
                        x.RefreshTimeInSeconds.Seconds,
                        (int)x.Status))
                .ToArray();

            await _hubContext.Clients.All.ReceiveMonitoringData(processedData);
        }
    }
}
