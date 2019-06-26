using System;

namespace SiteMonitoring.Domain
{
    /// <summary>
    /// Domain wrapper for site refresh time setting
    /// </summary>
    public class RefreshPeriod
    {
        private RefreshPeriod(ulong refreshTimeInSeconds)
        {
            if (refreshTimeInSeconds == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(refreshTimeInSeconds), "Refresh period should have positive value");
            }

            Seconds = refreshTimeInSeconds;
        }

        public static RefreshPeriod FromSeconds(ulong refreshTimeInSeconds) => new RefreshPeriod(refreshTimeInSeconds);

        public ulong Seconds { get; }
    }
}