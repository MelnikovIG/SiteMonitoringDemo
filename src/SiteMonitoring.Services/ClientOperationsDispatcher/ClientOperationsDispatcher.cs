using System.Threading.Tasks;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Services.ClientNotifier;
using SiteMonitoring.Services.IdGenerator;
using SiteMonitoring.Services.TrackingTimer;

namespace SiteMonitoring.Services.ClientOperationsDispatcher
{
    /// <summary>
    /// Client initialized operations dispatcher
    /// </summary>
    public class ClientOperationsDispatcher : IClientOperationsDispatcher
    {
        private readonly ISiteStorage _siteStorage;
        private readonly ITrackingTimer _trackingTimer;
        private readonly IIdGenerator _idGenerator;
        private readonly IClientNotifier _clientNotifier;

        public ClientOperationsDispatcher(
            ISiteStorage siteStorage,
            ITrackingTimer trackingTimer,
            IIdGenerator idGenerator,
            IClientNotifier clientNotifier)
        {
            _siteStorage = siteStorage;
            _trackingTimer = trackingTimer;
            _idGenerator = idGenerator;
            _clientNotifier = clientNotifier;
        }

        public async Task StartMonitoring(StartMonitoringModel startMonitoringModel)
        {
            var newId = _idGenerator.Generate();
            await _siteStorage.AddSiteInfo(newId, startMonitoringModel.Uri, startMonitoringModel.RefreshTimeInSeconds);
            await _trackingTimer.Start(newId, startMonitoringModel.Uri, startMonitoringModel.RefreshTimeInSeconds);
            await _clientNotifier.UpdateSitesInfo();
        }

        public async Task UpdateMonitoring(UpdateMonitoringModel updateMonitoringModel)
        {
            await _siteStorage.UpdateSiteInfo(updateMonitoringModel.Id, updateMonitoringModel.Uri, updateMonitoringModel.RefreshTimeInSeconds);
            await _trackingTimer.Update(updateMonitoringModel.Id, updateMonitoringModel.Uri, updateMonitoringModel.RefreshTimeInSeconds);
            await _clientNotifier.UpdateSitesInfo();
        }

        public async Task EndMonitoring(EndMonitoringModel endMonitoringModel)
        {
            await _siteStorage.RemoveSiteInfo(endMonitoringModel.Id);
            await _trackingTimer.Stop(endMonitoringModel.Id);
            await _clientNotifier.UpdateSitesInfo();
        }
    }
}