using System;
using Aps.AccountStatements;
using Aps.Fakes;
using Aps.Scheduling.ApplicationService;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Aps.Scraping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Aps.Scheduling.ApplicationService;
using Caliburn.Micro;
using Aps.Integration;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Serialization;
using Aps.BillingCompanies;
using System.Collections.Generic;
using System.ComponentModel;
using Aps.Integration.EnumTypes;
using Aps.Customers;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;

namespace Aps.Shared.Tests.CoreTests
{
    [TestClass]
    public class SchedulingEngineTest
    {

        Autofac.IContainer container;
        private Guid billingCompanyId;
        private Guid customerId;
        //private bool registrationType;
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
            builder.RegisterType<EventIntegrationRepositoryFake>().As<EventIntegrationRepositoryFake>();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();
            builder.RegisterType<ScrapeSessionInitiator>().As<ScrapeSessionInitiator>();
            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>();
            builder.RegisterType<CrossCheckScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.CrossCheckScrapper);
            builder.RegisterType<StatementScrapeOrchestrator>().Keyed<ScrapeOrchestrator>(ScrapeSessionTypes.StatementScrapper);

            container = builder.Build();
        }

        [TestMethod]
        public void CreatingNewSchedulingEngineObject()
        {
            // arrange

            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>());

            // act

            // assert
            Assert.AreEqual(schedulingEngine.maxAllowedServerScrapes, 20);
        }

        [TestMethod]
        public void getNumberOfThreadsAvailableOnServerIfZeroBillingCompanyThreadsAreUsedTest()
        {
            // arrange

            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>());

            // act
            int i = schedulingEngine.getNumberOfThreadsAvailableOnServer();

            // assert
            Assert.AreEqual(i, 20);
        }

        [TestMethod]
        public void getNumberOfThreadsAvailableOnServerIfSomeCompanyThreadsAreUsedTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>());

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

            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>());
            // act
            myList = schedulingEngine.getNewScrapeQueueWithoutCompletedItems();

            // assert
            Assert.AreEqual(myList, null);
        }

        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsWithOneEntryInRepoTest()
        {
            // arrange
            List<ScrapingObject> myList;
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>());
            ScrapingObject myScrapingObject = new ScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            //IScrapingObjectRepository myRepo = new ScrapingObjectRepositoryFake(container.Resolve<IEventAggregator>(), container.Resolve<ScrapingObjectCreator>());

            // act
            schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
            myList = schedulingEngine.MockGetAllScrapingObjects();

            // assert
            Assert.AreEqual(myList.Count, 1);
        }


        

    }
}
