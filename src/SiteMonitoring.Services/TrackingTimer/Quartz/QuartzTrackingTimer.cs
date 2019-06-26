using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using SiteMonitoring.Domain;

namespace SiteMonitoring.Services.TrackingTimer.Quartz
{
    public class QuartzTrackingTimer : ITrackingTimer
    {
        private const string _jobGroup = "jobGroup";
        private readonly IScheduler _scheduler;

        public QuartzTrackingTimer(IServiceProvider serviceProvider)
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            var schedulerFactory = new StdSchedulerFactory(props);
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.JobFactory = new JobFactory(serviceProvider);
            _scheduler.Start().GetAwaiter().GetResult();
        }

        public async Task Start(Guid siteId, Uri uri, RefreshPeriod refreshPeriod)
        {
            var job = CreateJobDetail(siteId, uri);

            var trigger = Create(siteId, refreshPeriod);

            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task Update(Guid siteId, Uri uri, RefreshPeriod refreshPeriod)
        {
            await Stop(siteId);
            await Start(siteId, uri, refreshPeriod);
        }

        public async Task Stop(Guid siteId)
        {
            await _scheduler.DeleteJob(new JobKey(siteId.ToString(), _jobGroup));
        }

        private static IJobDetail CreateJobDetail(Guid siteId, Uri uri)
        {
            var jobDetail = JobBuilder.Create<UpdateSiteStatusJob>()
                .WithIdentity(siteId.ToString(), _jobGroup)
                .Build();

            jobDetail.JobDataMap["id"] = siteId;
            jobDetail.JobDataMap["uri"] = uri;

            return jobDetail;
        }

        private static ITrigger Create(Guid siteId, RefreshPeriod refreshPeriod)
        {
            return TriggerBuilder.Create()
                .WithIdentity(siteId.ToString(), _jobGroup)
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds((int)refreshPeriod.Seconds).RepeatForever())
                .Build();
        }
    }
}
