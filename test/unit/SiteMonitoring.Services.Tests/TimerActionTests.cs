using System;
using System.Threading.Tasks;
using Moq;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.ClientNotifier;
using SiteMonitoring.Services.StatusChecker;
using SiteMonitoring.Services.TrackingTimer;
using Xunit;

namespace SiteMonitoring.Services.Tests
{
    /// <summary>
    /// Tests for <see cref="TimerAction"/>
    /// </summary>
    public class TimerActionTests
    {
        private readonly Mock<ISiteStorage> _siteStorageMock;
        private readonly Mock<IClientNotifier> _clientNotifierMock;
        private readonly Mock<ISiteStatusChecker> _siteStatusCheckerMock;
        private readonly TimerAction _target;

        public TimerActionTests()
        {
            _siteStatusCheckerMock = new Mock<ISiteStatusChecker>();
            _siteStorageMock = new Mock<ISiteStorage>();
            _clientNotifierMock = new Mock<IClientNotifier>();

            _target = new TimerAction(_siteStatusCheckerMock.Object, _siteStorageMock.Object, _clientNotifierMock.Object);
        }

        [Fact]
        public async Task Execute_Should_UpdateStorageStatus_If_StatusChanged()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var siteInfo = new SiteInfoEntity(siteId, siteUri, RefreshPeriod.FromSeconds(3));
            siteInfo.SetStatus(SiteStatus.Offline);

            _siteStorageMock.Setup(x => x.GetSiteInfo(siteId)).ReturnsAsync(siteInfo);
            _siteStatusCheckerMock.Setup(x => x.GetStatus(siteUri)).ReturnsAsync(StatusCheckResult.Success);

            //Act
            await _target.Execute(siteId, siteUri);

            //Assert
            _siteStorageMock.Verify(x => x.UpdateStatus(siteId, SiteStatus.Online), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldNot_UpdateStorageStatus_If_StatusNotChanged()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var siteInfo = new SiteInfoEntity(siteId, siteUri, RefreshPeriod.FromSeconds(3));
            siteInfo.SetStatus(SiteStatus.Online);

            _siteStorageMock.Setup(x => x.GetSiteInfo(siteId)).ReturnsAsync(siteInfo);
            _siteStatusCheckerMock.Setup(x => x.GetStatus(siteUri)).ReturnsAsync(StatusCheckResult.Success);

            //Act
            await _target.Execute(siteId, siteUri);

            //Assert
            _siteStorageMock.Verify(x => x.UpdateStatus(siteId, SiteStatus.Online), Times.Never);
        }

        [Fact]
        public async Task Execute_Should_NotifyClients_If_StatusChanged()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var siteInfo = new SiteInfoEntity(siteId, siteUri, RefreshPeriod.FromSeconds(3));
            siteInfo.SetStatus(SiteStatus.Offline);

            _siteStorageMock.Setup(x => x.GetSiteInfo(siteId)).ReturnsAsync(siteInfo);
            _siteStatusCheckerMock.Setup(x => x.GetStatus(siteUri)).ReturnsAsync(StatusCheckResult.Success);

            //Act
            await _target.Execute(siteId, siteUri);

            //Assert
            _clientNotifierMock.Verify(x => x.UpdateSitesInfo(), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldNot_NotifyClients_If_StatusNotChanged()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var siteInfo = new SiteInfoEntity(siteId, siteUri, RefreshPeriod.FromSeconds(3));
            siteInfo.SetStatus(SiteStatus.Online);

            _siteStorageMock.Setup(x => x.GetSiteInfo(siteId)).ReturnsAsync(siteInfo);
            _siteStatusCheckerMock.Setup(x => x.GetStatus(siteUri)).ReturnsAsync(StatusCheckResult.Success);

            //Act
            await _target.Execute(siteId, siteUri);

            //Assert
            _clientNotifierMock.Verify(x => x.UpdateSitesInfo(), Times.Never);
        }
    }
}
