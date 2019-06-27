using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteMonitoring.WebApp.SignalR
{
    public interface IMonitoringClient
    {
        Task ReceiveMonitoringData(ICollection<SiteInfoClientData> data);
    }
}
