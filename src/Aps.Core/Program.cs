using System;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.Events;
using Aps.Integration.Serialization;
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
            builder.RegisterType<AllBillingCompaniesQuery>().As<AllBillingCompaniesQuery>();
            builder.RegisterType<BillingCompanyOpenClosedWindowsQuery>().As<BillingCompanyOpenClosedWindowsQuery>();
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