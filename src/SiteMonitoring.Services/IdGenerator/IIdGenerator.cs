using System;

namespace SiteMonitoring.Services.IdGenerator
{
    /// <summary>
    /// Generates new unique identifier
    /// </summary>
    public interface IIdGenerator
    {
        Guid Generate();
    }
}
