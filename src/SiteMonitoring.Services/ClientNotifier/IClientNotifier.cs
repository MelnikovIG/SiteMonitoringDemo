using System.Threading.Tasks;

namespace SiteMonitoring.Services.ClientNotifier
{
    /// <summary>
    /// Client notifier
    /// </summary>
    public interface IClientNotifier
    {
        Task UpdateSitesInfo();
    }
}
