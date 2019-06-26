using System;

namespace SiteMonitoring.IntegrationTests.TestFacade
{
    public class SiteInfoWrapper
    {
        public SiteInfoWrapper(Guid id, Uri uri)
        {
            Id = id;
            Uri = uri;
        }

        public Guid Id { get; }
        public Uri Uri { get; }
    }
}