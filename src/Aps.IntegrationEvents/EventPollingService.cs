using System.Collections.Generic;

namespace Aps.IntegrationEvents
{
    public class EventPollingService
    {
        private readonly List<string> subscriptions;

        public EventPollingService()
        {
            subscriptions = new List<string>();
        }

        public void Subscribe(string eventName)
        {
            this.subscriptions.Add(eventName);
        }

        // select * from events where eventName in (subscriptionList) and event > lastEventNumber 
    }
}