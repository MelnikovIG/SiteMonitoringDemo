using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SiteMonitoring.Services.StatusChecker
{
    /// <summary>
    /// Site status checking service
    /// </summary>
    public class SiteStatusChecker : ISiteStatusChecker
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SiteStatusChecker(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<StatusCheckResult> GetStatus(Uri siteUri)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(1); // NOTE: can be moved to config

            try
            {
                var response = await client.GetAsync(siteUri);
                if (response.IsSuccessStatusCode)
                {
                    return StatusCheckResult.Success;

                }

                return StatusCheckResult.Failure;
            }
            catch (Exception e)
            {
                return StatusCheckResult.Failure;
            }
        }
    }
}