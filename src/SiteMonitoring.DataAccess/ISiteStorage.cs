using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiteMonitoring.Domain;

namespace SiteMonitoring.DataAccess
{
    /// <summary>
    /// Data access abstraction
    /// </summary>
    public interface ISiteStorage
    {
        Task<IReadOnlyCollection<SiteInfoEntity>> GetSitesInfo();

        Task<SiteInfoEntity> GetSiteInfo(Guid id);

        Task AddSiteInfo(Guid id, Uri uri, RefreshPeriod refreshPeriod);

        Task UpdateSiteInfo(Guid id, Uri uri, RefreshPeriod refreshPeriod);

        Task UpdateStatus(Guid id, SiteStatus status);

        Task RemoveSiteInfo(Guid id);

        Task ResetAllStatuses();
    }
}
