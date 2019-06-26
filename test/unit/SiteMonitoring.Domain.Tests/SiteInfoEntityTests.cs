using System;
using Xunit;

namespace SiteMonitoring.Domain.Tests
{
    /// <summary>
    /// Tests for <see  cref="SiteInfoEntity"/>
    /// </summary>
    public class SiteInfoEntityTests
    {
        [Fact]
        public void Ctor_ShouldSavePassedParams()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);

            //Act
            var entity = new SiteInfoEntity(siteId, siteUri, refreshTime);

            //Assert
            Assert.Equal(siteId, entity.Id);
            Assert.Equal(siteUri, entity.Uri);
            Assert.Equal(refreshTime, entity.RefreshTimeInSeconds);
        }

        [Fact]
        public void Ctor_Should_CreateEntity_WithUnknownStatus()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);

            //Act
            var entity = new SiteInfoEntity(siteId, siteUri, refreshTime);

            //Assert
            Assert.Equal(SiteStatus.Unknown, entity.Status);
        }

        [Fact]
        public void Ctor_ShouldFail_IfIdNotSet()
        {
            //Arrange
            var siteUri = new Uri($"http://{Guid.NewGuid()}");
            var refreshTime = RefreshPeriod.FromSeconds(4);

            //Act //Assert
            Assert.Throws<ArgumentOutOfRangeException>("id",() => new SiteInfoEntity(Guid.Empty, siteUri, refreshTime));
        }

        [Fact]
        public void Ctor_ShouldFail_IfUriIsNull()
        {
            //Arrange
            var refreshTime = RefreshPeriod.FromSeconds(4);

            //Act //Assert
            Assert.Throws<ArgumentNullException>("uri", () => new SiteInfoEntity(Guid.NewGuid(), null, refreshTime));
        }


        [Fact]
        public void Ctor_ShouldFail_IfRefreshTimeIsNull()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");

            //Act //Assert
            Assert.Throws<ArgumentNullException>("refreshTimeInSeconds", () => new SiteInfoEntity(siteId, siteUri, null));
        }

        [Fact]
        public void SetStatus_Should_ChangeStatus()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);
            var entity = new SiteInfoEntity(siteId, siteUri, refreshTime);
            var oldStatus = entity.Status;
            var newStatus = SiteStatus.Offline;

            //Act
            entity.SetStatus(newStatus);

            //Assert
            Assert.NotEqual(oldStatus, newStatus);
            Assert.Equal(entity.Status, newStatus);
        }
    }
}
