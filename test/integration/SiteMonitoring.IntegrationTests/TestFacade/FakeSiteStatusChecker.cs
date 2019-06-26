using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiteMonitoring.Services.StatusChecker;

namespace SiteMonitoring.IntegrationTests.TestFacade
{
    public class FakeSiteStatusChecker : ISiteStatusChecker
    {
        private readonly Dictionary<Uri, bool> _siteStatus = new Dictionary<Uri, bool>();

        public Task<StatusCheckResult> GetStatus(Uri siteUri)
        {
            if (!_siteStatus.ContainsKey(siteUri))
            {
                throw new Exception("Site status not configured for tests");
            }

            return Task.FromResult(_siteStatus[siteUri] ? StatusCheckResult.Success : StatusCheckResult.Failure);
        }

        public void MakeAvailable(Uri siteUri)
        {
            UpdateSiteInfo(siteUri, true);
        }

        public void MakeUnavailable(Uri siteUri)
        {
            UpdateSiteInfo(siteUri, false);
        }

        private void UpdateSiteInfo(Uri siteUri, bool isAvailable)
        {
            if (!_siteStatus.ContainsKey(siteUri))
            {
                _siteStatus.Add(siteUri, isAvailable);
            }
            else
            {
                _siteStatus[siteUri] = isAvailable;
            }
        }
    }
}