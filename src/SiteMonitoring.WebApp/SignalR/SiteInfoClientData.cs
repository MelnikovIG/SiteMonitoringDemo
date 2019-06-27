using System;

namespace SiteMonitoring.WebApp.SignalR
{
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
        public string Uri { get; }
        public ulong RefreshTimeInSeconds { get; }
        public int Status { get; }
    }
}