using System.Linq;
using System.Threading.Tasks;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Services.TrackingTimer;

namespace SiteMonitoring.Services.SystemOperationsDispatcher
{
    /// <summary>
    /// Dispatcher for operations, initialized by systems
    /// </summary>
    public class SystemOperationsDispatcher : ISystemOperationsDispatcher
    {
        private readonly ISiteStorage _siteStorage;
        private readonly ITrackingTimer _trackingTimer;

        public SystemOperationsDispatcher(ISiteStorage siteStorage, ITrackingTimer trackingTimer)
        {
            _siteStorage = siteStorage;
            _trackingTimer = trackingTimer;
        }
        public async Task StartTracking()
        {
            await _siteStorage.ResetAllStatuses();
            var sites = await _siteStorage.GetSitesInfo();
            var tasks = sites.Select(x => _trackingTimer.Start(x.Id, x.Uri, x.RefreshTimeInSeconds));
            await Task.WhenAll(tasks);
        }

    }
}