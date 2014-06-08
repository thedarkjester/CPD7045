using System;
using Aps.Scheduling.ApplicationService;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Caliburn.Micro;
using Aps.Integration.EnumTypes;
using Autofac.Features.Indexed;
using Moq;
using Aps.Scheduling.ApplicationService;
using Aps.Customers;
using Aps.BillingCompanies;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Aps.Scraping;

namespace Aps.Shared.Tests.CoreTests
{
    [TestClass]
    public class ScrapeSessionInitiatorTest
    {

        Mock eventAggregatorMock;
        Mock customerRepositoryMock;
        Mock billingCompanyRepositoryMock;
        Mock<IIndex<ScrapeSessionTypes, ScrapeOrchestrator>> mockIndex;
        Mock<StatementScrapeOrchestrator> mockStatementScrapeOrchestrator;
        Mock<CrossCheckScrapeOrchestrator> mockCrossCheckScrapeOrchestrator;
        Guid queueId;
        Guid customerId;
        Guid billingCompanyId;

        [TestInitialize]
        public void Setup()
        {
            billingCompanyId = Guid.NewGuid();
            customerId = Guid.NewGuid();
            queueId = Guid.NewGuid();
            eventAggregatorMock = new Mock<IEventAggregator>();
            customerRepositoryMock = new Mock<ICustomerRepository>();
            billingCompanyRepositoryMock = new Mock<IBillingCompanyRepository>();
            mockIndex = new Mock<IIndex<ScrapeSessionTypes, ScrapeOrchestrator>>();

            mockStatementScrapeOrchestrator = new Mock<StatementScrapeOrchestrator>(null, null, null, null);
            mockCrossCheckScrapeOrchestrator = new Mock<CrossCheckScrapeOrchestrator>(null, null, null);
            mockIndex.Setup(x => x[ScrapeSessionTypes.StatementScrapper]).Returns(mockStatementScrapeOrchestrator.Object);
            mockIndex.Setup(x => x[ScrapeSessionTypes.CrossCheckScrapper]).Returns(mockCrossCheckScrapeOrchestrator.Object);
        }


        [TestMethod]
        public void Given_StatementScrapeType_Then_Initiate_NewStatementScrapeSession()
        {
            var mockScrapeSessionInitiator = new Mock<ScrapeSessionInitiator>(eventAggregatorMock.Object, customerRepositoryMock.Object, billingCompanyRepositoryMock.Object, mockIndex.Object);
            
            ScrapeSessionInitiator scrapeSessionInitiator = mockScrapeSessionInitiator.Object;

            scrapeSessionInitiator.InitiateNewScrapeSession(new ScrapingObject(customerId, billingCompanyId, ScrapeSessionTypes.StatementScrapper));

            mockScrapeSessionInitiator.Verify();

        }

        [TestMethod]
        public void Given_CrossCheckScrapeType_Then_Initiate_NewStatementScrapeSession()
        {
            var mockScrapeSessionInitiator = new Mock<ScrapeSessionInitiator>(eventAggregatorMock.Object, customerRepositoryMock.Object, billingCompanyRepositoryMock.Object, mockIndex.Object);

            ScrapeSessionInitiator scrapeSessionInitiator = mockScrapeSessionInitiator.Object;

            scrapeSessionInitiator.InitiateNewScrapeSession(new ScrapingObject(customerId, billingCompanyId, ScrapeSessionTypes.CrossCheckScrapper));

            mockScrapeSessionInitiator.Verify();
        }

    }
}
