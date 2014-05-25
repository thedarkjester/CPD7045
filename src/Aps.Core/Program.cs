using System;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.IntegrationEvents;
using Aps.IntegrationEvents.Queries.BillingCompanyQueries;
using Aps.IntegrationEvents.Queries.Events;
using Aps.IntegrationEvents.Serialization;
using Autofac;
using Caliburn.Micro;
using Aps.Customer;

namespace Aps.Core
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

        private static void RegisterAllDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SchedulingEngine>().As<SchedulingEngine>().InstancePerDependency();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<CustomerRepositoryFake>().As<CustomerRepositoryFake>().InstancePerDependency();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<BillingCompanyRepositoryFake>().InstancePerDependency();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>().InstancePerDependency();
          
            RegisterIntegrationDependencies(builder);

            Container = builder.Build();
        }

        private static void RegisterQueries(ContainerBuilder builder)
        {
            builder.RegisterType<BillingCompanyByIdQuery>().As<BillingCompanyByIdQuery>();
            builder.RegisterType<BillingCompanyBillingLifeCycleByCompanyIdQuery>().As<BillingCompanyBillingLifeCycleByCompanyIdQuery>();
            builder.RegisterType<BillingCompanyScrapingUrlQuery>().As<BillingCompanyScrapingUrlQuery>();
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

            RegisterQueries(builder);
        }

        private static void StartMainApplication()
        {
            var schedulingEngine = Container.Resolve<SchedulingEngine>();
            schedulingEngine.Start();
        }

    
    }
}