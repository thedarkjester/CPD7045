using System.Collections.Generic;
using Aps.Integration.Events;

namespace Aps.Integration
{
    public interface IEventIntegrationRepository
    {
        void StoreEvent(IntegrationEvent integrationEvent);
        IEnumerable<IntegrationEvent> GetAllEvents();
        IEnumerable<IntegrationEvent> GetLatestEvents(int currentProcessedEvent, string nameSpaceName);
    }
}