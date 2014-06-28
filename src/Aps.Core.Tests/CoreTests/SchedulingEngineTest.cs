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
using Moq;
using Aps.Scheduling.ApplicationService.Extensions;
using Autofac.Features.Indexed;
using Aps.Scheduling.ApplicationService.Validation;
using Autofac.Core;
using Aps.Integration.Queries.Events;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Aps.Scraping.Scrapers;
using Aps.Scheduling.ApplicationService.Services;
using Aps.Scheduling.ApplicationService.Services;
using Aps.AccountStatements;
using Aps.Scraping.Scrapers;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;


namespace Aps.Shared.Tests.CoreTests
{
    [TestClass]
    public class SchedulingEngineTest
    {
        Autofac.IContainer container;
        private Guid billingCompanyId;
        private Guid customerId;
        ScrapeSessionTypes scrapeSessionTypes;
        Mock<IIndex<ScrapeSessionTypes, ScrapeOrchestrator>> mockIndex;
        
        [TestInitialize]
        public void Setup()
        {
            customerId = Guid.NewGuid();
            billingCompanyId = Guid.NewGuid();
            scrapeSessionTypes = ScrapeSessionTypes.StatementScrapper;
            mockIndex = new Mock<IIndex<ScrapeSessionTypes, ScrapeOrchestrator>>();

            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<BillingCompanyOpenClosedWindowsQuery>().As<BillingCompanyOpenClosedWindowsQuery>();
            builder.RegisterType<BillingCompanyScrapingLoadManagementConfigurationQuery>().As<BillingCompanyScrapingLoadManagementConfigurationQuery>();
            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>().SingleInstance();
            builder.RegisterType<ScrapingObjectRepositoryFake>().As<IScrapingObjectRepository>().SingleInstance();
            builder.RegisterType<ScrapingObjectCreator>().As<ScrapingObjectCreator>();
            builder.RegisterType<BinaryEventSerializer>().As<BinaryEventSerializer>();
            builder.RegisterType<BinaryEventDeSerializer>().As<BinaryEventDeSerializer>();
            builder.RegisterType<EventIntegrationRepositoryFake>().As<IEventIntegrationRepository>();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();


            ////////////////////////////////////////////////////////////////////////////////////////////
            builder.RegisterType<ScrapeSessionInitiator>().As<ScrapeSessionInitiator>(); // Jignesh's stuff that doesn't yet exist
            //builder.RegisterType<ScrapeSessionInitiatorFake>().As<ScrapeSessionInitiatorFake>(); // Mock object for testing
            builder.RegisterType<InvalidCredentialsValidator>().As<IValidator>();
            builder.RegisterType<DuplicateStatementValidator>().As<IValidator>();
            builder.RegisterType<ScrapeSessionDataValidator>().As<ScrapeSessionDataValidator>();

            
            ////////////////////////////////////////////////////////////////////////////////////////////
            builder.RegisterType<ScrapeSessionInitiator>().As<ScrapeSessionInitiator>();
            //builder.RegisterType<ScrapeSessionInitiatorFake>().As<ScrapeSessionInitiatorFake>(); // Mock object for testing
            builder.RegisterType<InvalidCredentialsValidator>().As<IValidator>().WithOrder();
            builder.RegisterType<DuplicateStatementValidator>().As<IValidator>().WithOrder();
            builder.RegisterType<ScrapeSessionDataValidator>().As<ScrapeSessionDataValidator>()

                .WithParameter(new ResolvedParameter((info, context) => true, (info, context) => context.ResolveOrdered<IValidator>()));
            builder.RegisterType<GetLatestEventsBySubScribedEventTypeQuery>()
                   .As<GetLatestEventsBySubScribedEventTypeQuery>()
                   .InstancePerDependency();


            builder.RegisterType<BillingCompanyByIdQuery>().As<BillingCompanyByIdQuery>();
            builder.RegisterType<BillingCompanyScrapingUrlQuery>().As<BillingCompanyScrapingUrlQuery>();
            builder.RegisterType<AllBillingCompaniesQuery>().As<AllBillingCompaniesQuery>();
            builder.RegisterType<CustomerBillingCompanyAccountsById>().As<CustomerBillingCompanyAccountsById>();
            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>().InstancePerDependency();
            builder.RegisterType<CustomerCreator>().As<CustomerCreator>().InstancePerDependency();
            builder.RegisterType<AccountStatementComposer>().As<AccountStatementComposer>().InstancePerDependency();
            builder.RegisterType<ScrapeLoggingRepositoryFake>().As<IScrapeLoggingRepository>().InstancePerDependency();
            builder.RegisterType<WebScraperFake>().As<IWebScraper>().InstancePerDependency();
            builder.RegisterType<CrossCheckScraperFake>().As<ICrossCheckScraper>().InstancePerDependency();

            ////////////////////////////////////////////////////////////////////////////////////////////


            /////////////////////////////////////////////////////////////////////////
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
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act

            // assert
            Assert.AreEqual(schedulingEngine.GetMaxNumberServerScrapesAllowed(), 20);
        }

        [TestMethod]
        public void defaultNumberOfConcurrentScrapesAllowedPerBillingCompanyTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            Guid myLocalBillingCompany = Guid.NewGuid();

            // act
            int i = schedulingEngine.getMaximumConcurrentThreadsAllowedForBillingCompanyAsDefinedByTheScrapingLoadManagementConfiguration(myLocalBillingCompany);

            // assert
            Assert.AreEqual(i, 5);
        }

        [TestMethod]
        public void getNumberOfThreadsAvailableOnServerIfZeroBillingCompanyThreadsAreUsedTest()
        {
            // arrange
           SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act
            int i = schedulingEngine.getNumberOfThreadsAvailableOnServer();

            // assert
            Assert.AreEqual(i, 20);
        }

        [TestMethod]
        public void getNumberOfThreadsAvailableOnServerIfSomeCompanyThreadsAreUsedTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

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
        public void getMaxAllowedThreadsForSpecificBillingCompanyDefaultValueIsFiveTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act
            Guid billingCompanyId = Guid.NewGuid();
            int i = schedulingEngine.getMaxAllowedThreadsForSpecificBillingCompany(null, billingCompanyId);

            // assert
            Assert.AreEqual(i, 5);
        }
        
        [TestMethod]
        public void currentOpenClosedWindowDtoByBillingCompanyReturnsNullIfItDoesNotExistTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

            // act
            Guid billingCompanyId = Guid.NewGuid();
            OpenClosedWindowDto openClosedWindowDto = schedulingEngine.GetCurrentOpenClosedWindowDtoByBillingCompany(billingCompanyId);

            // assert
            Assert.AreEqual(openClosedWindowDto, null);
        }

        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsIfEmptyTest()
        {
            // arrange
            List<ScrapingObject> myList;
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();

            // act
            myList = repo.GetAllScrapingObjectsSchedluedInThePast().ToList();

            // assert
            Assert.AreEqual(myList.Count, 0);
        }
        

        [TestMethod]
        public void getNewScrapeQueueWithoutCompletedItemsWithOneEntryInRepoTest()
        {
            // arrange
            List<ScrapingObject> myList;
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();

            // act
            repo.StoreScrapingObject(myScrapingObject);
            myList = repo.GetAllScrapingObjectsSchedluedInThePast().ToList();

            // assert
            Assert.AreEqual(myList.Count, 1);
        }

        
        [TestMethod]
        public void getNewScrapeQueueWithDefaultQueueingStrategyTest()
        {
            // arrange
            List<ScrapingObject> myList = new List<ScrapingObject>();
            List<ScrapingObject> myList2 = new List<ScrapingObject>();
            List<ScrapingObject> dummy = new List<ScrapingObject>();
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
           // IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            ScrapingObject myScrapingObject2 = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            ScrapingObject myScrapingObject3 = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, ScrapeSessionTypes.CrossCheckScrapper);
            IScrapeQueueingStrategy scrapeQueueingStrategy = schedulingEngine.GetScrapingQueueStrategy();

            myScrapingObject2.ScheduledDate = DateTime.UtcNow.AddDays(1);     

            // act
            myList2.Add(myScrapingObject);
            myList2.Add(myScrapingObject2);
            myList2.Add(myScrapingObject3);
            myList = scrapeQueueingStrategy.GetQueue(dummy, dummy, myList2).ToList();

            // assert
            Assert.AreEqual(myList.Count, 2);
            Assert.AreEqual(myList.ElementAt(0).QueueId, myScrapingObject3.QueueId);
            Assert.AreEqual(myList.ElementAt(1).QueueId, myScrapingObject.QueueId);
        }

        [TestMethod]
        public void getNewScrapeQueueWithOrederByCreatedDateStrategyTest()
        {
            // arrange
            List<ScrapingObject> myList = new List<ScrapingObject>();
            List<ScrapingObject> myList2 = new List<ScrapingObject>();
            List<ScrapingObject> dummy = new List<ScrapingObject>();
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
            InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            ScrapingObject myScrapingObject2 = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            ScrapingObject myScrapingObject3 = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, ScrapeSessionTypes.CrossCheckScrapper);
            

            myScrapingObject2.ScheduledDate = DateTime.UtcNow.AddDays(1);

            // act
            myList2.Add(myScrapingObject);
            myList2.Add(myScrapingObject2);
            myList2.Add(myScrapingObject3);
            mock.getScrapingQueueStrategy(new ScrapeQueueingStrategyByCreatedDate());
            IScrapeQueueingStrategy scrapeQueueingStrategy = schedulingEngine.GetScrapingQueueStrategy();
            myList = scrapeQueueingStrategy.GetQueue(dummy, dummy, myList2).ToList();

            // assert
            Assert.AreEqual(myList.Count, 2);
            Assert.AreEqual(myList.ElementAt(0).QueueId, myScrapingObject.QueueId);
            Assert.AreEqual(myList.ElementAt(1).QueueId, myScrapingObject3.QueueId);
        }
        
        [TestMethod]
        public void getcurrentNumberOfThreadsPerBillingCompanyReturnsZeroByDefaultTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());   

            // act
            Guid billingId = Guid.NewGuid();

            // assert
            Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingId), 0);    
        }
        
        [TestMethod]
        public void IncreaseNumberOfThreadsUsedByCompanyByCreatingMultipleThreadsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

             // act
             Guid billingId = Guid.NewGuid();
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingId);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingId), 3);
         }
        
        [TestMethod]
        public void CheckDecreaseNumberOfThreadsUsedByCompanyIsZeroWhenIfKeyDoesNotExistTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

             // act
             Guid billingId = Guid.NewGuid();
             schedulingEngine.DecreaseNumberOfThreadsUsedByCompany(billingId);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingId), 0);
         }
        
        [TestMethod]
        public void DecreaseNumberOfThreadsUsedByCompanyByCreatingAndRemovingMultipleThreadsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

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
        public void NumberOfThreadsCurrentlyUsedByBillingCompanyTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

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
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());

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
             List<ScrapingObject> myList = new List<ScrapingObject>();
             List<ScrapingObject> dummy = new List<ScrapingObject>();
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             IScrapeQueueingStrategy scrapeQueueingStrategy = schedulingEngine.GetScrapingQueueStrategy();
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
             repo.StoreScrapingObject(myScrapingObject1);
             repo.StoreScrapingObject(myScrapingObject2);
             repo.StoreScrapingObject(myScrapingObject3);
             repo.StoreScrapingObject(myScrapingObject4);
             repo.StoreScrapingObject(myScrapingObject5); 
             myList = repo.GetAllScrapingObjects().ToList();
             myList = scrapeQueueingStrategy.GetQueue(dummy, dummy, myList).ToList();

             // assert
             Assert.AreEqual(myList.Count, 3);
             Assert.AreEqual(myList.ElementAt(0).CustomerId, customerId2);
             Assert.AreEqual(myList.ElementAt(1).CustomerId, customerId5);
             Assert.AreEqual(myList.ElementAt(2).CustomerId, customerId3);
         }

        // TESTS FOR INTERNAL EVENTS VIA EVENTAGGREGATOR
        
         [TestMethod]
         public void CrossCheckEventSucceedsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             scrapeSessionTypes = ScrapeSessionTypes.CrossCheckScrapper;
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             repo.StoreScrapingObject(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);

             // act
             // if mock fails - remove from Repo
             // if mock true, change scrapeSessionTypes to StatementScraper
             // either, decrease number of threads used by billing company
             mock.getCrossCheckCompletedEvent(myScrapingObject.QueueId, true);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(myScrapingObject.ScrapeSessionTypes, ScrapeSessionTypes.StatementScrapper);
             Assert.AreEqual(repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId), myScrapingObject);
         }
        
         [TestMethod]
         public void CrossCheckEventFailsTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             scrapeSessionTypes = ScrapeSessionTypes.CrossCheckScrapper;
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             repo.StoreScrapingObject(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);

             // act
             // if mock fails - remove from Repo
             // if mock true, change scrapeSessionTypes to StatementScraper
             // either, decrease number of threads used by billing company
             mock.getCrossCheckCompletedEvent(myScrapingObject.QueueId, false);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId), null);
         }
        
         [TestMethod]
         public void ScrapeSessionDuplicateStatementTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             repo.StoreScrapingObject(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
             DateTime temp = myScrapingObject.ScheduledDate;

             // act
             // reschedule scrape
             // Keep in Repo
             // decrease number of threads used by billing company
             mock.getScrapeSessionDuplicateStatementEvent(myScrapingObject.QueueId);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId), myScrapingObject);
             ScrapingObject repoObject = repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId);
             Assert.AreNotEqual(repoObject.ScheduledDate, temp);
         }
        
         [TestMethod]
         public void ScrapeSessionSuccessfullTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             repo.StoreScrapingObject(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
             DateTime statementDateTime = DateTime.UtcNow.AddDays(-3);

             DateTime temp = myScrapingObject.ScheduledDate;

             // act
             // reschedule scrape
             // Keep in Repo
             // decrease number of threads used by billing company
             mock.getScrapeSessionSuccessfullEvent(myScrapingObject.QueueId, statementDateTime);

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId), myScrapingObject);
             ScrapingObject repoObject = repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId);
             Assert.AreNotEqual(repoObject.ScheduledDate, temp);
         }
        
         [TestMethod]
         public void ScrapeSessionFailedDueToScraperErrorTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();

             repo.StoreScrapingObject(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);


             // act
             // Decrease number of threads being used
             // Remove from Running List
             // reschedule scrape to DateTime.Max
             // Make scrapeStatus Inactive
             // Keep in Repo
             mock.getScrapeSessionFailedEvent(myScrapingObject.QueueId, ScrapingErrorResponseCodes.Unknown);
             // Error code tested: ScrapingErrorResponseCodes.Unknown
             // Error code tested: ScrapingErrorResponseCodes.UnhandledDataCondition
             // Error code tested: ScrapingErrorResponseCodes.BrokenScript

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(schedulingEngine.GetAllScrapeElementsRunningList().Count(), 0);
             Assert.AreEqual(myScrapingObject.ScheduledDate, DateTime.MaxValue);
             Assert.AreEqual(myScrapingObject.ScrapeStatus, "inactive");
             ScrapingObject repoObject = repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId);
             Assert.AreEqual(repoObject, myScrapingObject);

         }
        
        [TestMethod]
        public void ScrapeSessionFailedDueToUserErrorTest()
         {
             // arrange
             SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
             ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
             InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());
             ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
             IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();

             repo.StoreScrapingObject(myScrapingObject);
             schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
             schedulingEngine.AddScrapingElementToElementsRunningList(myScrapingObject);

             // act
             // Decrease number of threads being used
             // Remove from Running List
             // Add to completed List
             // Remove from Repo
             mock.getScrapeSessionFailedEvent(myScrapingObject.QueueId, ScrapingErrorResponseCodes.InvalidCredentials);
             // Error code tested: case ScrapingErrorResponseCodes.InvalidCredentials
             // Error code tested:    case ScrapingErrorResponseCodes.CustomerNotSignedUpForEBilling
             // Error code tested:    case ScrapingErrorResponseCodes.ActionRequiredbyBillingCompanyWebsite

             // assert
             Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
             Assert.AreEqual(schedulingEngine.GetAllScrapeElementsRunningList().Count(), 0);

             Assert.AreEqual(repo.GetCompletedScrapeQueue().ToList().Count(), 1);

             ScrapingObject repoObject = repo.GetScrapingObjectByQueueId(myScrapingObject.QueueId);
             Assert.AreEqual(repoObject, null);

         }
        
        [TestMethod]
        public void ScrapeSessionFailedDueToBillingCompanySiteDownErrorTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
            InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            DateTime tempDateTime = myScrapingObject.ScheduledDate;
            IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();

            repo.StoreScrapingObject(myScrapingObject);
            schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
            schedulingEngine.AddScrapingElementToElementsRunningList(myScrapingObject);

            // act
            // Decrease number of threads being used
            // Remove from Running List
            // Reschedule Scrape
            mock.getScrapeSessionFailedEvent(myScrapingObject.QueueId, ScrapingErrorResponseCodes.BillingCompanySiteDown);

            // assert
            Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
            Assert.AreEqual(schedulingEngine.GetAllScrapeElementsRunningList().Count(), 0);
            Assert.IsTrue(myScrapingObject.ScheduledDate >= tempDateTime.AddHours(12));

        }
        
        [TestMethod]
        public void ScrapeSessionFailedDueToErrorPageEncounteredErrorTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            ScrapingObjectCreator scrapingObjectCreator = new ScrapingObjectCreator(container.Resolve<IEventAggregator>());
            InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());
            ScrapingObject myScrapingObject = scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
            DateTime tempDateTime = myScrapingObject.ScheduledDate;
            IScrapingObjectRepository repo = schedulingEngine.GetScrapingObjectRepositoryFake();

            repo.StoreScrapingObject(myScrapingObject);
            schedulingEngine.IncreaseNumberOfThreadsUsedByCompany(billingCompanyId);
            schedulingEngine.AddScrapingElementToElementsRunningList(myScrapingObject);

            // act
            // Decrease number of threads being used
            // Remove from Running List
            // Reschedule Scrape
            mock.getScrapeSessionFailedEvent(myScrapingObject.QueueId, ScrapingErrorResponseCodes.ErrorPageEncountered);


            // assert
            Assert.AreEqual(schedulingEngine.NumberOfThreadsCurrentlyUsedByBillingCompanyId(billingCompanyId), 0);
            Assert.AreEqual(schedulingEngine.GetAllScrapeElementsRunningList().Count(), 0);
            Assert.IsTrue(myScrapingObject.ScheduledDate >= tempDateTime.AddHours(6));
        }

        [TestMethod]
        public void NumberOfServerThreadsAvailableTest()
        {
            // arrange
            SchedulingEngine schedulingEngine = new SchedulingEngine(container.Resolve<IEventAggregator>(), container.Resolve<EventIntegrationService>(), container.Resolve<IScrapingObjectRepository>(), container.Resolve<BillingCompanyOpenClosedWindowsQuery>(), container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>(), container.Resolve<ScrapeSessionInitiator>(), container.Resolve<ScrapingErrorRetryConfigurationQuery>(), container.Resolve<ScrapingObjectCreator>(), container.Resolve<BillingCompanyCrossCheckEnabledByIdQuery>(), container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>());
            InternalSchedularEventsMock mock = new InternalSchedularEventsMock(container.Resolve<IEventAggregator>());

            int i = schedulingEngine.GetMaxNumberServerScrapesAllowed();

            // act
            // Decrease number of max Server Threads allowed via Event
            mock.getMaxServerThreadModificationEvent(15);
            int j = schedulingEngine.GetMaxNumberServerScrapesAllowed();

            // assert
            Assert.AreEqual(i, 20);
            Assert.AreEqual(j, 15);
        }

    }
}
