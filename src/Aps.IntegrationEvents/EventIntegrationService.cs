using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Aps.IntegrationEvents.Serialization;
using Caliburn.Micro;

namespace Aps.IntegrationEvents
{
    public class EventIntegrationService
    {
        private readonly IEventAggregator eventAggregator;
        private readonly BinaryEventSerializer eventSerializer;
        private readonly BinaryEventDeSerializer binaryEventDeSerializer;
        private readonly List<string> subscriptions;

        public EventIntegrationService(IEventAggregator eventAggregator, BinaryEventSerializer eventSerializer,BinaryEventDeSerializer binaryEventDeSerializer)
        {
            this.eventAggregator = eventAggregator;
            this.eventSerializer = eventSerializer;
            this.binaryEventDeSerializer = binaryEventDeSerializer;
            this.eventAggregator.Subscribe(this);

            subscriptions = new List<string>();

            StartPolling();
        }

        private void StartPolling()
        {
            // every so often look for events
            // transforms the external event to a domain event

            // for each in subscription list check DB
            // if event valid

            //eventAggregator.Publish(DeserializedEventData);
        }

        public void Subscribe(string eventName)
        {
            this.subscriptions.Add(eventName);
        }

        // select * from events where eventName in (subscriptionList) and event > lastEventNumber 
        public void Publish(object message)
        {
            //get type name for someone to search on
            var messageType = message.GetType().FullName;
            
            // serialize data from the message
            byte[] data = eventSerializer.SerializeMessage(message);
            // store in the DB for someone to pick up serializing the data based on the type
        }

    }

   
    [Serializable]
    public class ScrapeSessionFailedEvent
    {
        Guid ScrapeSessionId { get; set; }

        string FailureReason { get; set; }
    }
}