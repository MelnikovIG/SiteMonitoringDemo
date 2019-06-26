using System.Threading.Tasks;
using SiteMonitoring.Services.ClientNotifier;

namespace SiteMonitoring.IntegrationTests.TestFacade
{
    public class FakeClientNotifier : IClientNotifier
    {
        private int _receivedEventsCount;
        private int _readEventsCount;

        public Task UpdateSitesInfo()
        {
            _receivedEventsCount++;
            return Task.CompletedTask;
        }

        public bool ReadEvent()
        {
            if (_readEventsCount < _receivedEventsCount)
            {
                _readEventsCount++;
                return true;
            }

            return false;
        }
    }
}