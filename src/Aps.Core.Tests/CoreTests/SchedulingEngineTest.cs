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
using System.Linq;

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
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
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
            schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);

            Guid billingCompanyId2 = Guid.NewGuid();
            schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId2);

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
            myList = schedulingEngine.MockGetAllScrapingObjectsScheduledInPast();

            // assert
            Assert.AreEqual(myList.Count, 0);
        }

        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsWithOneEntryInRepoTest()
        {
            // arrange
            List<ScrapingObject> myList;
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            //ScrapingObject myScrapingObject = new ScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            

            // act
            schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
            myList = schedulingEngine.MockGetAllScrapingObjectsScheduledInPast();

            // assert
            Assert.AreEqual(myList.Count, 1);
        }


        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsContainingOneValidObjectIgnoringFutureSchedulesTest()
        {
            // arrange
            List<ScrapingObject> myList;
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            ScrapingObject myScrapingObject2 = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);

            
            myScrapingObject2.ScheduledDate = DateTime.UtcNow.AddDays(1);     


            // act
            schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
            schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject2);
            myList = schedulingEngine.getNewScrapeQueueWithoutCompletedItems();

            // assert
            Assert.AreEqual(myList.Count, 1);
            Assert.AreEqual(myList.ElementAt(0).queueId, myScrapingObject.queueId);
            Assert.AreNotEqual(myList.ElementAt(0).queueId, myScrapingObject2.queueId);
        }

         [TestMethod]
        public void IncreaseNumberOfThreadsUsedByCompanyByCreatingFirstValueTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act
            Guid billingId = Guid.NewGuid();
            schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);

            // assert
            Assert.AreEqual(schedulingEngine.currentNumberOfThreadsPerBillingCompany.ContainsKey(billingId), true);
        }

         [TestMethod]
         public void IncreaseNumberOfThreadsUsedByCompanyByCreatingMultipleThreadsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

             // act
             Guid billingId = Guid.NewGuid();
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);

             // assert
             Assert.AreEqual(schedulingEngine.currentNumberOfThreadsPerBillingCompany[billingId], 3);
         }

         [TestMethod]
         public void CheckDecreaseNumberOfThreadsUsedByCompanyIsZeroWhenIfKeyDoesNotExistTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

             // act
             Guid billingId = Guid.NewGuid();
             schedulingEngine.DecreaseNumberOfThreadsUsedByCompany(billingId);

             // assert
             Assert.AreEqual(schedulingEngine.currentNumberOfThreadsPerBillingCompany[billingId], 0);
         }

         [TestMethod]
         public void DecreaseNumberOfThreadsUsedByCompanyByCreatingAndRemovingMultipleThreadsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

             // act
             Guid billingId = Guid.NewGuid();
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.DecreaseNumberOfThreadsUsedByCompany(billingId);

             // assert
             Assert.AreEqual(schedulingEngine.currentNumberOfThreadsPerBillingCompany[billingId], 2);
         }

         [TestMethod]
         public void NumberOfThreadsCurrentlyUsedByBillingCompanyTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

             // act
             Guid billingId = Guid.NewGuid();
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.DecreaseNumberOfThreadsUsedByCompany(billingId);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingId), 2);
         }

         [TestMethod]
         public void RescheduleItemTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             DateTime myDate = myScrapingObject.ScheduledDate.AddDays(1);


             // act
             schedulingEngine.RescheduleItem(myScrapingObject, myDate);

             

             // assert
             Assert.AreEqual(myScrapingObject.ScheduledDate, myDate);
         }

         [TestMethod]
         public void getNewScrapeQueueWithoutCompletedItemsWithMulipleScrapeObjectsWithRandomParametersTest()
         {
             // Mothod should sort by:
             // OrderBy(item => item.scrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.createdDate)
             // arrange
             List<ScrapingObject> myList;
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             Guid customerId1 = Guid.NewGuid();
             Guid customerId2 = Guid.NewGuid();
             Guid customerId3 = Guid.NewGuid();
             Guid customerId4 = Guid.NewGuid();
             Guid customerId5 = Guid.NewGuid();
             ScrapeSessionTypes crossCheck = ScrapeSessionTypes.CrossCheckScrapper;
             ScrapeSessionTypes normalScrape = ScrapeSessionTypes.StatementScrapper;

             ScrapingObject myScrapingObject1 = scrapingObjectCreator.GetNewScrapingObject(customerId1, billingCompanyId, normalScrape); 
             ScrapingObject myScrapingObject2 = scrapingObjectCreator.GetNewScrapingObject(customerId2, billingCompanyId, crossCheck);
             ScrapingObject myScrapingObject3 = scrapingObjectCreator.GetNewScrapingObject(customerId3, billingCompanyId, normalScrape);
             ScrapingObject myScrapingObject4 = scrapingObjectCreator.GetNewScrapingObject(customerId4, billingCompanyId, normalScrape);
             ScrapingObject myScrapingObject5 = scrapingObjectCreator.GetNewScrapingObject(customerId5, billingCompanyId, crossCheck);

             myScrapingObject1.ScheduledDate = DateTime.UtcNow.AddDays(1);
             myScrapingObject2.ScheduledDate = DateTime.UtcNow.AddDays(-1);
             myScrapingObject3.ScheduledDate = DateTime.UtcNow.AddDays(-2);
             myScrapingObject4.ScheduledDate = DateTime.UtcNow.AddDays(3);
             myScrapingObject5.ScheduledDate = DateTime.UtcNow;


             // act
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject1);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject2);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject3);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject4);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject5);

             myList = schedulingEngine.getNewScrapeQueueWithoutCompletedItems();

             // assert
             Assert.AreEqual(myList.Count, 3);
             Assert.AreEqual(myList.ElementAt(0).customerId, customerId2);
             Assert.AreEqual(myList.ElementAt(1).customerId, customerId5);
             Assert.AreEqual(myList.ElementAt(2).customerId, customerId3);
         }

         [TestMethod]
         public void CrossCheckEventSucceedsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             scrapeSessionTypes = ScrapeSessionTypes.CrossCheckScrapper;
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);

             // act
             // if mock fails - remove from Repo
             // if mock true, change scrapeSessionTypes to StatementScraper
             // either, decrease number of threads used by billing company
             mock.getCrossCheckCompletedEvent(myScrapingObject.queueId, true);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(myScrapingObject.scrapeSessionTypes, ScrapeSessionTypes.StatementScrapper);
             Assert.AreEqual(schedulingEngine.mockGetScrapeObjectByQueueId(myScrapingObject.queueId), myScrapingObject);
         }

         [TestMethod]
         public void CrossCheckEventFailsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             scrapeSessionTypes = ScrapeSessionTypes.CrossCheckScrapper;
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);

             // act
             // if mock fails - remove from Repo
             // if mock true, change scrapeSessionTypes to StatementScraper
             // either, decrease number of threads used by billing company
             mock.getCrossCheckCompletedEvent(myScrapingObject.queueId, false);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(schedulingEngine.mockGetScrapeObjectByQueueId(myScrapingObject.queueId), null);
         }

         [TestMethod]
         public void ScrapeSessionDuplicateStatementTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
             DateTime temp = myScrapingObject.ScheduledDate;

             // act
             // reschedule scrape
             // Keep in Repo
             // decrease number of threads used by billing company
             mock.getScrapeSessionDuplicateStatementEvent(myScrapingObject.queueId);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(schedulingEngine.mockGetScrapeObjectByQueueId(myScrapingObject.queueId), myScrapingObject);
             ScrapingObject repoObject =  schedulingEngine.mockGetScrapeObjectByQueueId(myScrapingObject.queueId);
             Assert.AreNotEqual(repoObject.ScheduledDate, temp);
         }

         [TestMethod]
         public void ScrapeSessionSuccessfullTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiatorFake>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             schedulingEngine.mockAddBillingCompanyAccountAdded(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
             DateTime statementDateTime = DateTime.UtcNow.AddDays(-3);

             DateTime temp = myScrapingObject.ScheduledDate;

             // act
             // reschedule scrape
             // Keep in Repo
             // decrease number of threads used by billing company
             mock.getScrapeSessionSuccessfullEvent(myScrapingObject.queueId, statementDateTime);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(schedulingEngine.mockGetScrapeObjectByQueueId(myScrapingObject.queueId), myScrapingObject);
             ScrapingObject repoObject = schedulingEngine.mockGetScrapeObjectByQueueId(myScrapingObject.queueId);
             Assert.AreNotEqual(repoObject.ScheduledDate, temp);
         }

    }
}
