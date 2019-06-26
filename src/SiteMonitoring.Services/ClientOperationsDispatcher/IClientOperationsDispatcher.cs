using System.Threading.Tasks;

namespace SiteMonitoring.Services.ClientOperationsDispatcher
{
    /// <summary>
    /// Client initialized operations dispatcher
    /// </summary>
    public interface IClientOperationsDispatcher
    {
        Task StartMonitoring(StartMonitoringModel startMonitoringModel);
        Task UpdateMonitoring(UpdateMonitoringModel updateMonitoringModel);
        Task EndMonitoring(EndMonitoringModel endMonitoringModel);
    }
}
