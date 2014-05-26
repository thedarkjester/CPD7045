﻿using System;
using Aps.BillingCompanies;
using Aps.Customer;
using Aps.IntegrationEvents;
using Aps.IntegrationEvents.Queries.Events;
using Aps.IntegrationEvents.Serialization;
using Autofac;
using Caliburn.Micro;

namespace Aps.Audit
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            // construct the dependency injection builder that when built, will structure and "know"
            // the dependency hierarchy/chains
            RegisterAllDependencies();

            StartMainApplication();

            Console.ReadLine();
        }

        private static void StartMainApplication()
        {
            // start audit listener etc.
        }

        private static void RegisterAllDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<CustomerRepositoryFake>().As<CustomerRepositoryFake>().InstancePerDependency();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<BillingCompanyRepositoryFake>().InstancePerDependency();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>().InstancePerDependency();

            RegisterIntegrationDependencies(builder);

            Container = builder.Build();
        }

        private static void RegisterIntegrationDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>().SingleInstance();
            builder.RegisterType<BinaryEventSerializer>().As<BinaryEventSerializer>().InstancePerDependency();
            builder.RegisterType<BinaryEventDeSerializer>().As<BinaryEventDeSerializer>().InstancePerDependency();
            builder.RegisterType<EventIntegrationRepositoryFake>().As<EventIntegrationRepositoryFake>().InstancePerDependency();
            builder.RegisterType<GetLatestEventsBySubScribedEventTypeQuery>()
                   .As<GetLatestEventsBySubScribedEventTypeQuery>()
                   .InstancePerDependency();
        }
    }
}