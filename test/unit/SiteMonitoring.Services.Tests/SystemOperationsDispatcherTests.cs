using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SiteMonitoring.DataAccess;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.SystemOperationsDispatcher;
using SiteMonitoring.Services.TrackingTimer;
using Xunit;

namespace SiteMonitoring.Services.Tests
{
    /// <summary>
    /// Tests for <see cref="SystemOperationsDispatcher"/>
    /// </summary>
    public class SystemOperationsDispatcherTests
    {
        private readonly Mock<ISiteStorage> _siteStorageMock;
        private readonly Mock<ITrackingTimer> _trackingTimerMock;
        private SystemOperationsDispatcher.SystemOperationsDispatcher _target;

        public SystemOperationsDispatcherTests()
        {
            _siteStorageMock = new Mock<ISiteStorage>();
            _trackingTimerMock = new Mock<ITrackingTimer>();

            _target = new SystemOperationsDispatcher.SystemOperationsDispatcher(_siteStorageMock.Object,_trackingTimerMock.Object);
        }

        [Fact]
        public async Task StartTracking_Should_ResetStorageSiteStatuses()
        {
            //Arrange
            _siteStorageMock.Setup(x => x.GetSitesInfo()).ReturnsAsync(new List<SiteInfoEntity>());

            //Act
            await _target.StartTracking();

            //Assert
            _siteStorageMock.Verify(x => x.ResetAllStatuses(), Times.Once);
        }

        [Fact]
        public async Task StartTracking_Should_RunTrackingTimers()
        {
            //Arrange
            var site1Id = Guid.NewGuid();
            var site1Uri = new Uri($"http://{site1Id}");
            var refreshTime1 = RefreshPeriod.FromSeconds(4);

            var site2Id = Guid.NewGuid();
            var site2Uri = new Uri($"http://{site2Id}");
            var refreshTime2 = RefreshPeriod.FromSeconds(3);
            _siteStorageMock.Setup(x => x.GetSitesInfo()).ReturnsAsync(new List<SiteInfoEntity>()
            {
                new SiteInfoEntity(site1Id, site1Uri, refreshTime1),
                new SiteInfoEntity(site2Id, site2Uri, refreshTime2)
            });

            //Act
            await _target.StartTracking();

            //Assert
            _trackingTimerMock.Verify(x => x.Start(site1Id, site1Uri, refreshTime1), Times.Once);
            _trackingTimerMock.Verify(x => x.Start(site2Id, site2Uri, refreshTime2), Times.Once);
        }
    }
}
