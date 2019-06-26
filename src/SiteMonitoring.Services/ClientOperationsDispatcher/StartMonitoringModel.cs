using System;
using SiteMonitoring.Domain;

namespace SiteMonitoring.Services.ClientOperationsDispatcher
{
    public class StartMonitoringModel
    {
        public StartMonitoringModel(Uri uri, RefreshPeriod refreshPeriod)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            RefreshTimeInSeconds = refreshPeriod ?? throw new ArgumentNullException(nameof(refreshPeriod));
        }

        public Uri Uri { get; }
        public RefreshPeriod RefreshTimeInSeconds { get; }
    }
}