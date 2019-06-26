using System;
using System.Linq;
using System.Threading.Tasks;
using SiteMonitoring.DataAccess.Exceptions;
using SiteMonitoring.Domain;
using Xunit;

namespace SiteMonitoring.DataAccess.Tests
{
    /// <summary>
    /// Tests for <see  cref="InMemorySiteStorage"/>
    /// </summary>
    public class InMemorySiteStorageTests
    {
        private readonly InMemorySiteStorage _storage;

        public InMemorySiteStorageTests()
        {
            _storage = new InMemorySiteStorage();
        }

        [Fact]
        public async Task GetSitesInfo_Should_Return_AllSitesInfo()
        {
            //Arrange
            var site1Id = Guid.NewGuid();
            var site1Uri = new Uri($"http://{site1Id}");
            var refreshTime1 = RefreshPeriod.FromSeconds(4);

            var site2Id = Guid.NewGuid();
            var site2Uri = new Uri($"http://{site2Id}");
            var refreshTime2 = RefreshPeriod.FromSeconds(3);

            await _storage.AddSiteInfo(site1Id, site1Uri, refreshTime1);
            await _storage.AddSiteInfo(site2Id, site2Uri, refreshTime2);

            //Act
            var result = await _storage.GetSitesInfo();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(site1Id, result.Select(x => x.Id));
            Assert.Contains(site2Id, result.Select(x => x.Id));
        }

        [Fact]
        public async Task GetSiteInfo_Should_ReturnAllSiteInfo()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);

            await _storage.AddSiteInfo(siteId, siteUri, refreshTime);

            //Act 
            var result = await _storage.GetSiteInfo(siteId);

            //Assert
            Assert.Equal(siteId, result.Id);
            Assert.Equal(siteUri, result.Uri);
            Assert.Equal(refreshTime, result.RefreshTimeInSeconds);
            Assert.Equal(SiteStatus.Unknown, result.Status);
        }

        [Fact]
        public async Task GetSiteInfo_Should_Fail_If_IdNotExist()
        {
            //Arrange
            var siteId = Guid.NewGuid();

            //Act //Assert
            await Assert.ThrowsAsync<EntityNotExistException>(async () => await _storage.GetSiteInfo(siteId));
        }

        [Fact]
        public async Task AddSiteInfo_ShouldFail_If_IdAlreadyAdded()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);

            await _storage.AddSiteInfo(siteId, siteUri, refreshTime);

            //Act //Assert
            await Assert.ThrowsAsync<EntityAlreadyExistException>(async () =>
                await _storage.AddSiteInfo(siteId, siteUri, refreshTime));
        }

        [Fact]
        public async Task UpdateSiteInfo_Should_ApplyNewInfo()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);
            await _storage.AddSiteInfo(siteId, siteUri, refreshTime);

            var siteUri2 = new Uri($"http://{Guid.NewGuid()}");
            var refreshTime2 = RefreshPeriod.FromSeconds(5);

            //Act
            await _storage.UpdateSiteInfo(siteId, siteUri2, refreshTime2);

            //Assert
            var updatedSiteData = await _storage.GetSiteInfo(siteId);
            Assert.Equal(siteId, updatedSiteData.Id);
            Assert.Equal(siteUri2, updatedSiteData.Uri);
            Assert.Equal(refreshTime2, updatedSiteData.RefreshTimeInSeconds);
            Assert.Equal(SiteStatus.Unknown, updatedSiteData.Status);
        }

        [Fact]
        public async Task UpdateSiteInfo_Should_Fail_If_IdNotExist()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);

            //Act //Assert
            await Assert.ThrowsAsync<EntityNotExistException>(async () => await _storage.UpdateSiteInfo(siteId, siteUri, refreshTime));
        }

        [Fact]
        public async Task UpdateStatus_Should_ApplyNewStatus()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);
            await _storage.AddSiteInfo(siteId, siteUri, refreshTime);

            var oldStatus = (await _storage.GetSiteInfo(siteId)).Status;
            var newStatus = SiteStatus.Offline;

            //Act
            await _storage.UpdateStatus(siteId, newStatus);

            //Assert
            Assert.NotEqual(oldStatus, newStatus);
            Assert.Equal(SiteStatus.Offline, newStatus);
        }

        [Fact]
        public async Task UpdateStatus_Should_Fail_If_IdNotExist()
        {
            //Arrange
            var siteId = Guid.NewGuid();

            //Act //Assert
            await Assert.ThrowsAsync<EntityNotExistException>(async () => await _storage.UpdateStatus(siteId, SiteStatus.Offline));
        }

        [Fact]
        public async Task RemoveSiteInfo_Should_RemoveEntityFromStorage()
        {
            //Arrange
            var siteId = Guid.NewGuid();
            var siteUri = new Uri($"http://{siteId}");
            var refreshTime = RefreshPeriod.FromSeconds(4);
            await _storage.AddSiteInfo(siteId, siteUri, refreshTime);

            //Act
            await _storage.RemoveSiteInfo(siteId);

            //Assert
            var siteInfos = await _storage.GetSitesInfo();
            Assert.DoesNotContain(siteId, siteInfos.Select(x => x.Id));
        }

        [Fact]
        public async Task RemoveSiteInfo_Should_Fail_If_IdNotExist()
        {
            //Arrange
            var siteId = Guid.NewGuid();

            //Act //Assert
            await Assert.ThrowsAsync<EntityNotExistException>(async () => await _storage.RemoveSiteInfo(siteId));
        }

        [Fact]
        public async Task ResetAllStatuses_Should_ResetAllSitesStatusToUnknown()
        {
            //Arrange
            var site1Id = Guid.NewGuid();
            var site1Uri = new Uri($"http://{site1Id}");
            var refreshTime1 = RefreshPeriod.FromSeconds(4);

            var site2Id = Guid.NewGuid();
            var site2Uri = new Uri($"http://{site2Id}");
            var refreshTime2 = RefreshPeriod.FromSeconds(3);

            await _storage.AddSiteInfo(site1Id, site1Uri, refreshTime1);
            await _storage.AddSiteInfo(site2Id, site2Uri, refreshTime2);

            await _storage.UpdateStatus(site1Id, SiteStatus.Offline);
            await _storage.UpdateStatus(site2Id, SiteStatus.Online);

            //Act
            await _storage.ResetAllStatuses();

            //Assert
            var sitesIfo = await _storage.GetSitesInfo();
            foreach (var siteInfoEntity in sitesIfo)
            {
                Assert.Equal(SiteStatus.Unknown, siteInfoEntity.Status);
            }
        }
    }
}
