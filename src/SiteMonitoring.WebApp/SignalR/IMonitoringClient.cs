using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteMonitoring.WebApp.SignalR
{
    public interface IMonitoringClient
    {
        Task ReceiveMonitoringData(ICollection<SiteInfoClientData> data);
    }

    public class SiteInfoClientData
    {
        public SiteInfoClientData(Guid id, string uri, ulong refreshTimeInSeconds, int status)
        {
            Id = id;
            Uri = uri;
            RefreshTimeInSeconds = refreshTimeInSeconds;
            Status = status;
        }

        public Guid Id { get; }
        public String Uri { get; }
        public ulong RefreshTimeInSeconds { get; }
        public int Status { get; }
    }
}
