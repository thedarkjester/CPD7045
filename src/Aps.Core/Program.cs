﻿using System;
using Aps.BillingCompanies;
using Aps.Core.Services;
using Aps.Fakes;
using Aps.Integration;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.Events;
using Aps.Integration.Serialization;
using Autofac;
using Caliburn.Micro;
using Aps.Customers;
using Aps.Core.ScrapeOrchestrators;
using Aps.Integration.EnumTypes;
using Aps.Scraping;
using Aps.Scraping.Scrapers;

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

            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>().InstancePerDependency();
            builder.RegisterType<CustomerCreator>().As<CustomerCreator>().InstancePerDependency();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>().InstancePerDependency();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>().InstancePerDependency();
            builder.RegisterType<AccountStatementComposer>().As<AccountStatementComposer>().InstancePerDependency();
            builder.RegisterType<ScrapeLoggingRepositoryFake>().As<IScrapeLoggingRepository>().InstancePerDependency();
            builder.RegisterType<WebScraperFake>().As<IWebScraper>().InstancePerDependency();
            builder.RegisterType<CrossCheckScraperFake>().As<ICrossCheckScraper>().InstancePerDependency();
            builder.RegisterType<CrossCheckScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.CrossCheckScrapper);
            builder.RegisterType<StatementScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.StatementScrapper);
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
            builder.RegisterType<ScrapingErrorRetryConfigurationQuery>().As<ScrapingErrorRetryConfigurationQuery>();
            builder.RegisterType<BillingCompanyScrapingLoadManagementConfigurationQuery>().As<BillingCompanyScrapingLoadManagementConfigurationQuery>();
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