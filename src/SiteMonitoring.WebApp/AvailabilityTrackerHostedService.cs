using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SiteMonitoring.Domain;
using SiteMonitoring.Services.ClientOperationsDispatcher;
using SiteMonitoring.Services.SystemOperationsDispatcher;

namespace SiteMonitoring.WebApp
{
    public class AvailabilityTrackerHostedService : BackgroundService
    {
        private readonly ISystemOperationsDispatcher _systemOperationsDispatcher;
        private readonly IClientOperationsDispatcher _clientOperationsDispatcher;

        public AvailabilityTrackerHostedService(
            ISystemOperationsDispatcher systemOperationsDispatcher,
            IClientOperationsDispatcher clientOperationsDispatcher)
        {
            _systemOperationsDispatcher = systemOperationsDispatcher;
            _clientOperationsDispatcher = clientOperationsDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _systemOperationsDispatcher.StartTracking();

            await _clientOperationsDispatcher.StartMonitoring(new StartMonitoringModel(new Uri("http://ya.ru"), RefreshPeriod.FromSeconds(3)));
            await _clientOperationsDispatcher.StartMonitoring(new StartMonitoringModel(new Uri("http://notexisturl1234567890.ru"), RefreshPeriod.FromSeconds(4)));
            await _clientOperationsDispatcher.StartMonitoring(new StartMonitoringModel(new Uri("http://localhost:5000/alwaysOk"), RefreshPeriod.FromSeconds(5)));
            await _clientOperationsDispatcher.StartMonitoring(new StartMonitoringModel(new Uri("http://localhost:5000/alwaysFail"), RefreshPeriod.FromSeconds(5)));
            await _clientOperationsDispatcher.StartMonitoring(new StartMonitoringModel(new Uri("http://localhost:5000/alwaysLong"), RefreshPeriod.FromSeconds(5)));
            await _clientOperationsDispatcher.StartMonitoring(new StartMonitoringModel(new Uri("http://localhost:5000/random"), RefreshPeriod.FromSeconds(1)));
        }
    }
}