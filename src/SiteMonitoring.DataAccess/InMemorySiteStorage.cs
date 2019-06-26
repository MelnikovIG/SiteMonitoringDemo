using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteMonitoring.DataAccess.Exceptions;
using SiteMonitoring.Domain;

namespace SiteMonitoring.DataAccess
{
    /// <summary>
    /// In memory implementation of storage
    /// </summary>
    public class InMemorySiteStorage : ISiteStorage
    {
        private readonly Dictionary<Guid, SiteInfoEntity> _siteData = new Dictionary<Guid, SiteInfoEntity>();
        private readonly object _lockObject = new object();

        public Task<IReadOnlyCollection<SiteInfoEntity>> GetSitesInfo()
        {
            IReadOnlyCollection<SiteInfoEntity> data = _siteData.Values.Select(x => x).ToList();
            return Task.FromResult(data);
        }

        public Task<SiteInfoEntity> GetSiteInfo(Guid id)
        {
            if (!_siteData.ContainsKey(id))
            {
                throw new EntityNotExistException();
            }

            return Task.FromResult(_siteData[id]);
        }

        public Task AddSiteInfo(Guid id, Uri uri, RefreshPeriod refreshPeriod)
        {
            lock (_lockObject)
            {
                if (_siteData.ContainsKey(id))
                {
                    throw new EntityAlreadyExistException();
                }

                _siteData.TryAdd(id, new SiteInfoEntity(id, uri, refreshPeriod));
                return Task.CompletedTask;
            }
        }

        public Task UpdateSiteInfo(Guid id, Uri uri, RefreshPeriod refreshPeriod)
        {
            lock (_lockObject)
            {
                if (!_siteData.ContainsKey(id))
                {
                    throw new EntityNotExistException();
                }

                var siteInfo = new SiteInfoEntity(id, uri, refreshPeriod);
                _siteData[id] = siteInfo;

                return Task.CompletedTask;
            }
        }

        public Task UpdateStatus(Guid id, SiteStatus status)
        {
            lock (_lockObject)
            {
                if (!_siteData.ContainsKey(id))
                {
                    throw new EntityNotExistException();
                }

                _siteData[id].SetStatus(status);
                return Task.CompletedTask;
            }
        }

        public Task RemoveSiteInfo(Guid id)
        {
            lock (_lockObject)
            {
                if (!_siteData.ContainsKey(id))
                {
                    throw new EntityNotExistException();
                }

                return Task.FromResult(_siteData.Remove(id, out _));
            }
        }

        public Task ResetAllStatuses()
        {
            lock (_lockObject)
            {
                foreach (var siteInfoEntity in _siteData)
                {
                    siteInfoEntity.Value.SetStatus(SiteStatus.Unknown);
                }

                return Task.CompletedTask;
            }
        }
    }
}
