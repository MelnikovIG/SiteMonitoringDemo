using System;
using System.Threading.Tasks;
using SiteMonitoring.Domain;

namespace SiteMonitoring.Services.TrackingTimer
{
    public interface ITrackingTimer
    {
        Task Start(Guid siteId, Uri uri, RefreshPeriod refreshPeriod);
        Task Update(Guid siteId, Uri uri, RefreshPeriod refreshPeriod);
        Task Stop(Guid siteId);
    }
}
