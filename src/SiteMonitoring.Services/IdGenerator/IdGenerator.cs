using System;

namespace SiteMonitoring.Services.IdGenerator
{
    /// <summary>
    /// Generates new unique identifier
    /// </summary>
    public class IdGenerator : IIdGenerator
    {
        public Guid Generate() => Guid.NewGuid();
    }
}