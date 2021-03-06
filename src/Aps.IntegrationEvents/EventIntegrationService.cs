﻿using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Aps.Integration.Events;
using Aps.Integration.Serialization;
using Caliburn.Micro;

namespace Aps.Integration
{
    public class EventIntegrationService
    {
        private int currentProcessedEvent = 0;

        private readonly IEventAggregator eventAggregator;
        private readonly BinaryEventSerializer eventSerializer;
        private readonly BinaryEventDeSerializer binaryEventDeSerializer;
        private readonly IEventIntegrationRepository eventIntegrationRepository;
        private readonly List<string> subscriptions;
        CancellationToken cancellationToken;

        public EventIntegrationService(IEventAggregator eventAggregator,
            BinaryEventSerializer eventSerializer,
            BinaryEventDeSerializer binaryEventDeSerializer,
            IEventIntegrationRepository eventIntegrationRepository)
        {
            this.eventAggregator = eventAggregator;
            this.eventSerializer = eventSerializer;
            this.binaryEventDeSerializer = binaryEventDeSerializer;
            this.eventIntegrationRepository = eventIntegrationRepository;
            this.eventAggregator.Subscribe(this);

            subscriptions = new List<string>();
            cancellationToken = new CancellationToken(false);

            Task.Factory.StartNew(StartPolling, cancellationToken);

            Thread.Sleep(1000);
        }

       protected virtual void StartPolling()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Getting External Events");

                foreach (var subscription in subscriptions)
                {
                    IEnumerable<IntegrationEvent> events = eventIntegrationRepository.GetLatestEvents(currentProcessedEvent, subscription);
                    DispatchEventsInProcess(events);
                }

                Console.WriteLine("Dispatching External Events Internally");
                Console.WriteLine("Pausing for 2500 milliseconds post processing");

                // this should be configurable
                Thread.Sleep(2500);
            }
        }

        protected virtual void DispatchEventsInProcess(IEnumerable<IntegrationEvent> events)
        {
            foreach (var integrationEvent in events)
            {
                var deserializedEvent = binaryEventDeSerializer.DeSerializeMessage(integrationEvent.SerializedEvent);
                eventAggregator.Publish(deserializedEvent);

                if (integrationEvent.RowVersion > currentProcessedEvent)
                {
                    currentProcessedEvent = integrationEvent.RowVersion;
                    // store somewhere for future persistence so we don't reload events each time we start the instance.
                }
            }
        }

        public void SubscribeToEventByNameSpace(string eventName)
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

            // store for someone to pick up serializing the data based on the type
            eventIntegrationRepository.StoreEvent(new IntegrationEvent(messageType, data));
        }

    }
}