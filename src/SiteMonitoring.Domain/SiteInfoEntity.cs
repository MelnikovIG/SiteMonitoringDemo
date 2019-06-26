using System;

namespace SiteMonitoring.Domain
{
    /// <summary>
    /// Domain entity for site info
    /// </summary>
    public class SiteInfoEntity
    {
        public SiteInfoEntity(Guid id, Uri uri, RefreshPeriod refreshTimeInSeconds)
        {
            Id = id != default ? id : throw new ArgumentOutOfRangeException(nameof(id));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            RefreshTimeInSeconds = refreshTimeInSeconds ?? throw new ArgumentNullException(nameof(refreshTimeInSeconds));
            Status = SiteStatus.Unknown;
        }

        public void SetStatus(SiteStatus status)
        {
            Status = status;
        }

        public Guid Id { get; }
        public Uri Uri { get; }
        public SiteStatus Status { get; private set; }
        public RefreshPeriod RefreshTimeInSeconds { get; }
    }
}