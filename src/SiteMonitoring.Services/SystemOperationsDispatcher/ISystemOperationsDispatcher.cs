using System.Threading.Tasks;

namespace SiteMonitoring.Services.SystemOperationsDispatcher
{
    /// <summary>
    /// Dispatcher for operations, initialized by systems
    /// </summary>
    public interface ISystemOperationsDispatcher
    {
        Task StartTracking();
    }
}
