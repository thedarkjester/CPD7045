﻿using System.Collections.Generic;
using System.Linq;
using Aps.Integration;
using Aps.Integration.Events;

namespace Aps.Fakes
{
    public class EventIntegrationRepositoryFake : IEventIntegrationRepository
    {
        private readonly List<IntegrationEvent> events;

        public EventIntegrationRepositoryFake()
        {
            events = new List<IntegrationEvent>();
        }

        public void StoreEvent(IntegrationEvent integrationEvent)
        {
            events.Add(integrationEvent);

            // currently used to fake out the database rowversioning
            integrationEvent.SetRowVersion(events.Count);
        }

        public IEnumerable<IntegrationEvent> GetAllEvents()
        {
            return events;
        }

        public IEnumerable<IntegrationEvent> GetLatestEvents(int currentProcessedEvent, string nameSpaceName)
        {
            List<IntegrationEvent> returnedEvents = events.Where(x => x.NameSpaceName == nameSpaceName && x.RowVersion > currentProcessedEvent).ToList();

            return returnedEvents;
        }
    }
}