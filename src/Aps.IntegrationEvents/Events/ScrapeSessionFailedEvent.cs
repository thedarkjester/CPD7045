using System;

namespace Aps.Integration.Events
{
    [Serializable]
    public class ScrapeSessionFailedEvent
    {
        Guid ScrapeSessionId { get; set; }

        string FailureReason { get; set; }
    }
}