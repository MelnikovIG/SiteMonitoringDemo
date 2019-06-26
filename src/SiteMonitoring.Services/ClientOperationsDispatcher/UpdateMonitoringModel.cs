using System;
using SiteMonitoring.Domain;

namespace SiteMonitoring.Services.ClientOperationsDispatcher
{
    public class UpdateMonitoringModel
    {
        public UpdateMonitoringModel(Guid id, Uri uri, RefreshPeriod refreshPeriod)
        {
            Id = id != default ? id : throw new ArgumentOutOfRangeException(nameof(id));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            RefreshTimeInSeconds = refreshPeriod ?? throw new ArgumentNullException(nameof(refreshPeriod));
        }

        public Guid Id { get; }
        public Uri Uri { get; }
        public RefreshPeriod RefreshTimeInSeconds { get; }
    }
}