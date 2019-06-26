using System;

namespace SiteMonitoring.Services.ClientOperationsDispatcher
{
    public class EndMonitoringModel
    {
        public EndMonitoringModel(Guid id)
        {
            Id = id != default ? id : throw new ArgumentOutOfRangeException(nameof(id));
        }

        public Guid Id { get; }
    }
}