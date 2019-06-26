using System;
using System.Threading.Tasks;
using Moq;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.ClientNotifier;
using SiteMonitoring.Services.ClientOperationsDispatcher;
using SiteMonitoring.Services.IdGenerator;
using SiteMonitoring.Services.TrackingTimer;
using Xunit;

namespace SiteMonitoring.Services.Tests
{
    /// <summary>
    /// Tests for <see cref="ClientOperationsDispatcher"/>
    /// </summary>
    public class ClientOperationsDispatcherTests
    {
        private readonly Mock<ISiteStorage> _siteStorageMock;
        private readonly Mock<ITrackingTimer> _trackingTimerMock;
        private readonly Mock<IClientNotifier> _clientNotifierMock;
        private readonly Guid _siteId;
        private readonly Uri _siteUri;
        private readonly RefreshPeriod _refreshTime;
        private readonly ClientOperationsDispatcher.ClientOperationsDispatcher _target;

        public ClientOperationsDispatcherTests()
        {
            _siteStorageMock = new Mock<ISiteStorage>();
            _trackingTimerMock = new Mock<ITrackingTimer>();
            _clientNotifierMock = new Mock<IClientNotifier>();
            var idGeneratorMock = new Mock<IIdGenerator>();

            _siteId = Guid.NewGuid();
            _siteUri = new Uri($"http://{_siteId}");
            _refreshTime = RefreshPeriod.FromSeconds(4);
            idGeneratorMock.Setup(x => x.Generate()).Returns(_siteId);

            _target = new ClientOperationsDispatcher.ClientOperationsDispatcher(
                _siteStorageMock.Object,
                _trackingTimerMock.Object,
                idGeneratorMock.Object,
                _clientNotifierMock.Object);
        }

        [Fact]
        public async Task StartMonitoring_Should_AddDataToStorage()
        {
            //Arrange
            var startMonitoringModel = new StartMonitoringModel(_siteUri, _refreshTime);

            //Act
            await _target.StartMonitoring(startMonitoringModel);

            //Asset
            _siteStorageMock.Verify(x => x.AddSiteInfo(_siteId, _siteUri, _refreshTime), Times.Once);
        }

        [Fact]
        public async Task StartMonitoring_Should_StartTrackingTimer()
        {
            //Arrange
            var startMonitoringModel = new StartMonitoringModel(_siteUri, _refreshTime);

            //Act
            await _target.StartMonitoring(startMonitoringModel);

            //Asset
            _trackingTimerMock.Verify(x => x.Start(_siteId, _siteUri, _refreshTime), Times.Once);
        }

        [Fact]
        public async Task StartMonitoring_Should_NotifyClients()
        {
            //Arrange
            var startMonitoringModel = new StartMonitoringModel(_siteUri, _refreshTime);

            //Act
            await _target.StartMonitoring(startMonitoringModel);

            //Asset
            _clientNotifierMock.Verify(x => x.UpdateSitesInfo(), Times.Once);
        }

        [Fact]
        public async Task UpdateMonitoring_Should_UpdateStorageData()
        {
            //Arrange
            var updateMonitoringModel = new UpdateMonitoringModel(_siteId, _siteUri, _refreshTime);

            //Act
            await _target.UpdateMonitoring(updateMonitoringModel);

            //Asset
            _siteStorageMock.Verify(x => x.UpdateSiteInfo(_siteId, _siteUri, _refreshTime), Times.Once);
        }

        [Fact]
        public async Task UpdateMonitoring_Should_UpdateTrackingTimer()
        {
            //Arrange
            var updateMonitoringModel = new UpdateMonitoringModel(_siteId, _siteUri, _refreshTime);

            //Act
            await _target.UpdateMonitoring(updateMonitoringModel);

            //Asset
            _trackingTimerMock.Verify(x => x.Update(_siteId, _siteUri, _refreshTime), Times.Once);
        }

        [Fact]
        public async Task UpdateMonitoring_Should_NotifyClients()
        {
            //Arrange
            var updateMonitoringModel = new UpdateMonitoringModel(_siteId, _siteUri, _refreshTime);

            //Act
            await _target.UpdateMonitoring(updateMonitoringModel);

            //Asset
            _clientNotifierMock.Verify(x => x.UpdateSitesInfo(), Times.Once);
        }

        [Fact]
        public async Task EndMonitoring_Should_RemoveDataFromStorage()
        {
            //Arrange
            var endMonitoringModel = new EndMonitoringModel(_siteId);

            //Act
            await _target.EndMonitoring(endMonitoringModel);

            //Asset
            _siteStorageMock.Verify(x => x.RemoveSiteInfo(_siteId), Times.Once);
        }

        [Fact]
        public async Task EndMonitoring_Should_StopTrackingTimer()
        {
            //Arrange
            var endMonitoringModel = new EndMonitoringModel(_siteId);

            //Act
            await _target.EndMonitoring(endMonitoringModel);

            //Asset
            _trackingTimerMock.Verify(x => x.Stop(_siteId), Times.Once);
        }

        [Fact]
        public async Task EndMonitoring_Should_NotifyClients()
        {
            //Arrange
            var endMonitoringModel = new EndMonitoringModel(_siteId);

            //Act
            await _target.EndMonitoring(endMonitoringModel);

            //Asset
            _clientNotifierMock.Verify(x => x.UpdateSitesInfo(), Times.Once);
        }
    }
}
