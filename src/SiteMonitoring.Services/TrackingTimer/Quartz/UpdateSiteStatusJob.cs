using System;
using System.Threading.Tasks;
using Quartz;

namespace SiteMonitoring.Services.TrackingTimer.Quartz
{
    [DisallowConcurrentExecution]
    public class UpdateSiteStatusJob : IJob
    {
        private readonly ITimerAction _timerAction;

        public UpdateSiteStatusJob(ITimerAction timerAction)
        {
            _timerAction = timerAction;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Guid id = (Guid)context.JobDetail.JobDataMap["id"];
            Uri uri = (Uri)context.JobDetail.JobDataMap["uri"];

            await _timerAction.Execute(id, uri);

            Console.WriteLine($"{DateTime.Now} Job executed {uri}");
        }
    }
}