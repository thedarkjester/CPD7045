using System;
using Aps.Fakes;
using Aps.Scheduling.ApplicationService;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Aps.Scraping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Caliburn.Micro;
using Aps.Integration;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Serialization;
using Aps.BillingCompanies;
using System.Collections.Generic;
using Aps.Integration.EnumTypes;
using Aps.Customers;

namespace Aps.Shared.Tests.CoreTests
{
    [TestClass]
    public class SchedulingEngineTest
    {

        Autofac.IContainer container;
        private Guid billingCompanyId;
        private Guid customerId;
        ScrapeSessionTypes scrapeSessionTypes;

        [TestInitialize]
        public void Setup()
        {
            customerId = Guid.NewGuid();
            billingCompanyId = Guid.NewGuid();
            //registrationType = false;
            scrapeSessionTypes = ScrapeSessionTypes.StatementScrapper;

            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<BillingCompanyOpenClosedWindowsQuery>().As<BillingCompanyOpenClosedWindowsQuery>();
            builder.RegisterType<BillingCompanyScrapingLoadManagementConfigurationQuery>().As<BillingCompanyScrapingLoadManagementConfigurationQuery>();
            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>();
            builder.RegisterType<ScrapingObjectRepositoryFake>().As<IScrapingObjectRepository>().SingleInstance();
            builder.RegisterType<ScrapingObjectCreator>().As<ScrapingObjectCreator>();

            builder.RegisterType<BinaryEventSerializer>().As<BinaryEventSerializer>();
            builder.RegisterType<BinaryEventDeSerializer>().As<BinaryEventDeSerializer>();

            builder.RegisterType<EventIntegrationRepositoryFake>().As<IEventIntegrationRepository>();

            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();

            //builder.RegisterType<ScrapeSessionInitiator>().As<ScrapeSessionInitiator>(); // Jignesh's stuff that doesn't yet exist
            builder.RegisterType<ScrapeSessionInitiatorFake>().As<ScrapeSessionInitiatorFake>(); // Mock object for testing

            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>();

            builder.RegisterType<CrossCheckScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.CrossCheckScrapper);
            builder.RegisterType<StatementScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.StatementScrapper);

            builder.RegisterType<ScrapingErrorRetryConfigurationQuery>().As<ScrapingErrorRetryConfigurationQuery>();
            builder.RegisterType<ScrapingObjectCreator>().As<ScrapingObjectCreator>();
            builder.RegisterType<BillingCompanyCrossCheckEnabledByIdQuery>().As<BillingCompanyCrossCheckEnabledByIdQuery>();
            builder.RegisterType<BillingCompanyBillingLifeCycleByCompanyIdQuery>().As<BillingCompanyBillingLifeCycleByCompanyIdQuery>();

            container = builder.Build();

        }

        [TestMethod]
        public void CreatingNewSchedulingEngineObject()
        {
            // arrange

            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act

            // assert
            Assert.AreEqual(schedulingEngine.maxAllowedServerScrapes, 20);
        }

        [TestMethod]
        public void getNumberOfThreadsAvailableOnServerIfZeroBillingCompanyThreadsAreUsedTest()
        {
            // arrange

            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act
            int i = schedulingEngine.getNumberOfThreadsAvailableOnServer();

            // assert
            Assert.AreEqual(i, 20);
        }

        [TestMethod]
        public void getNumberOfThreadsAvailableOnServerIfSomeCompanyThreadsAreUsedTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act
            Guid billingCompanyId = Guid.NewGuid();
            schedulingEngine.addNewCurrentNumberOfThreadsPerBillingCompany(billingCompanyId);

            Guid billingCompanyId2 = Guid.NewGuid();
            schedulingEngine.addNewCurrentNumberOfThreadsPerBillingCompany(billingCompanyId2);

            schedulingEngine.currentNumberOfThreadsPerBillingCompany[billingCompanyId] += 1;
            schedulingEngine.currentNumberOfThreadsPerBillingCompany[billingCompanyId2] += 1;

            int i = schedulingEngine.getNumberOfThreadsAvailableOnServer();

            // assert
            Assert.AreEqual(i, 18);
        }

        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsIfEmptyTest()
        {
            // arrange
            List<ScrapingObject> myList;

            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            // act
            myList = schedulingEngine.MockGetAllScrapingObjects();

            // assert
            Assert.AreEqual(myList.Count, 0);
        }

        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsWithOneEntryInRepoTest()
        {
            // arrange
            List<ScrapingObject> myList;
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObject myScrapingObject = new ScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            

            // act
            schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
            myList = schedulingEngine.MockGetAllScrapingObjects();

            // assert
            Assert.AreEqual(myList.Count, 1);
        }


        

    }
}
