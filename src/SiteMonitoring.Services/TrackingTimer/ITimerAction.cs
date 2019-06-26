using System;
using System.Threading.Tasks;

namespace SiteMonitoring.Services.TrackingTimer
{
    /// <summary>
    /// Logic, executed by timer tick
    /// </summary>
    public interface ITimerAction
    {
        Task Execute(Guid siteId, Uri siteUri);
    }
}
