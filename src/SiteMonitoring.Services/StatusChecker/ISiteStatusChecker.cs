using System;
using System.Threading.Tasks;

namespace SiteMonitoring.Services.StatusChecker
{
    /// <summary>
    /// Site status checking service
    /// </summary>
    public interface ISiteStatusChecker
    {
        Task<StatusCheckResult> GetStatus(Uri siteUri);
    }
}
