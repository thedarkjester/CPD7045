using System;
using Aps.BillingCompanies;
using Aps.Fakes;
using Aps.Integration;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.Events;
using Aps.Integration.Serialization;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Aps.Scheduling.ApplicationService.Services;
using Autofac;
using Caliburn.Micro;
using Aps.Customers;
using Aps.Integration.EnumTypes;
using Aps.Scraping;
using Aps.Scraping.Scrapers;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Aps.Scheduling.ApplicationService.Extensions;
using Aps.Scheduling.ApplicationService.Validation;
using Aps.AccountStatements;
using Autofac.Core;
using Aps.Scheduling.ApplicationService.Interpreters;

namespace Aps.Scheduling.ApplicationService
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
            builder.RegisterType<AccountStatementRepositoryFake>().As<IAccountStatementRepository>().InstancePerDependency();            
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>().InstancePerDependency();
            builder.RegisterType<AccountStatementComposer>().As<AccountStatementComposer>().InstancePerDependency();
            builder.RegisterType<ScrapeLoggingRepositoryFake>().As<IScrapeLoggingRepository>().InstancePerDependency();
            builder.RegisterType<ScrapeSessionXMLToDataPairConverter>().AsSelf().InstancePerDependency();
            builder.RegisterType<WebScraperFake>().As<IWebScraper>().InstancePerDependency();
            builder.RegisterType<CrossCheckScraperFake>().As<ICrossCheckScraper>().InstancePerDependency();
            builder.RegisterType<CrossCheckScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.CrossCheckScrapper);
            builder.RegisterType<StatementScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.StatementScrapper);

            RegisterValidators(builder);
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
            builder.RegisterType<CustomerBillingCompanyAccountsById>().As<CustomerBillingCompanyAccountsById>();
            builder.RegisterType<BillingCompanyCrossCheckEnabledByIdQuery>().As<BillingCompanyCrossCheckEnabledByIdQuery>();
            
        }

        private static void RegisterIntegrationDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ScrapingObjectRepositoryFake>().As<IScrapingObjectRepository>().SingleInstance();
            builder.RegisterType<ScrapingObjectCreator>().As<ScrapingObjectCreator>().SingleInstance();
            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>().SingleInstance();
            builder.RegisterType<BinaryEventSerializer>().As<BinaryEventSerializer>().InstancePerDependency();
            builder.RegisterType<BinaryEventDeSerializer>().As<BinaryEventDeSerializer>().InstancePerDependency();
            builder.RegisterType<EventIntegrationRepositoryFake>().As<IEventIntegrationRepository>().InstancePerDependency();
            builder.RegisterType<ScrapeSessionInitiator>().As<ScrapeSessionInitiator>().InstancePerDependency();
            builder.RegisterType<ScrapeSessionInitiatorFake>().As<ScrapeSessionInitiatorFake>().InstancePerDependency();
            builder.RegisterType<GetLatestEventsBySubScribedEventTypeQuery>()
                   .As<GetLatestEventsBySubScribedEventTypeQuery>()
                   .InstancePerDependency();

            RegisterQueries(builder);
        }

        private static void RegisterValidators(ContainerBuilder builder)
        {
            builder.RegisterType<InvalidCredentialsValidator>().As<IValidator>().WithOrder();
            builder.RegisterType<DuplicateStatementValidator>().As<IValidator>().WithOrder();

            builder.RegisterType<ScrapeSessionDataValidator>().As<ScrapeSessionDataValidator>()
                .WithParameter(new ResolvedParameter((info, context) => true, (info, context) => context.ResolveOrdered<IValidator>()));
        }

        private static void StartMainApplication()
        {
            Console.WriteLine("Starting scheduling engine");

            var schedulingEngine = Container.Resolve<SchedulingEngine>();
            schedulingEngine.Start();
        }
    }
}