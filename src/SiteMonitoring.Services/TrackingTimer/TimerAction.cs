using System;
using System.Threading.Tasks;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.ClientNotifier;
using SiteMonitoring.Services.StatusChecker;

namespace SiteMonitoring.Services.TrackingTimer
{
    /// <summary>
    /// Logic, executed by timer tick
    /// </summary>
    public class TimerAction : ITimerAction
    {
        private readonly ISiteStatusChecker _siteStatusChecker;
        private readonly ISiteStorage _siteStorage;
        private readonly IClientNotifier _clientNotifier;

        public TimerAction(ISiteStatusChecker siteStatusChecker, ISiteStorage siteStorage, IClientNotifier clientNotifier)
        {
            _siteStatusChecker = siteStatusChecker;
            _siteStorage = siteStorage;
            _clientNotifier = clientNotifier;
        }

        public async Task Execute(Guid siteId, Uri siteUri)
        {
            var status = await _siteStatusChecker.GetStatus(siteUri);
            var newSiteStatus = status == StatusCheckResult.Success ? SiteStatus.Online : SiteStatus.Offline;

            var siteInfo = await _siteStorage.GetSiteInfo(siteId);
            if (siteInfo.Status != newSiteStatus)
            {
                await _siteStorage.UpdateStatus(siteId, newSiteStatus);
                await _clientNotifier.UpdateSitesInfo();
            }
        }
    }
}