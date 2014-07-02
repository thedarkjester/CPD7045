using System;
using Aps.BillingCompanies.ValueObjects;
using Aps.Scheduling.ApplicationService;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caliburn.Micro;
using Aps.Integration.EnumTypes;
using Autofac.Features.Indexed;
using Moq;
using Aps.Customers;
using Aps.BillingCompanies;
using Aps.Scraping;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Domain.Tests.CoreTests
{
    [TestClass]
    public class ScrapeSessionInitiatorTest
    {

        Mock<CustomerBillingCompanyAccountsById> customerBillingCompanyAccountsByIdMock;
        Mock<BillingCompanyScrapingUrlQuery> billingCompanyScrapingUrlQueryMock;
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
            var eventAggregator = new Mock<IEventAggregator>();
            var customerRepository =  new Mock<ICustomerRepository>();

            var customer =new Customers.Aggregates.Customer(eventAggregator.Object, new Customers.ValueObjects.CustomerFirstName("User"), new Customers.ValueObjects.CustomerLastName("Test"), null, null, null, null);
            var customerBillingCompanyAccount = new Customers.Entities.CustomerBillingCompanyAccount(billingCompanyId, "x", "y", "active", "1235", 1, DateTime.Now.AddHours(1));
            customerBillingCompanyAccount.CustomerStatement = new Customers.ValueObjects.CustomerStatement(Guid.NewGuid(), DateTime.Now.AddHours(1));
            customer.AddCustomerBillingCompanyAccount(customerBillingCompanyAccount);

            customerRepository.Setup(x => x.GetCustomerById(customerId)).Returns(customer);
          
            
            customerBillingCompanyAccountsByIdMock = new Mock<CustomerBillingCompanyAccountsById>(customerRepository.Object);
            customerBillingCompanyAccountsByIdMock.SetReturnsDefault<CustomerBillingCompanyAccountDto>(new CustomerBillingCompanyAccountDto());
            
            var billingCompanyRepository = new Mock<IBillingCompanyRepository>();
            billingCompanyRepository.Setup(x => x.GetBillingCompanyById(billingCompanyId)).Returns(new BillingCompanies.Aggregates.BillingCompany(eventAggregator.Object, new BillingCompanies.ValueObjects.BillingCompanyName("Telkom"), new BillingCompanies.ValueObjects.BillingCompanyType(1), new BillingCompanies.ValueObjects.BillingCompanyScrapingUrl("https://www.telkom.co.za"), new BillingCompanyCrossCheckScrapeEnabled(false)));
            
            billingCompanyScrapingUrlQueryMock = new Mock<BillingCompanyScrapingUrlQuery>(billingCompanyRepository.Object);
            billingCompanyScrapingUrlQueryMock.SetReturnsDefault<BillingCompanyScrapingUrlDto>(new BillingCompanyScrapingUrlDto());
          
            
            mockIndex = new Mock<IIndex<ScrapeSessionTypes, ScrapeOrchestrator>>();

            mockStatementScrapeOrchestrator = new Mock<StatementScrapeOrchestrator>(null, null, null, null, null, null, null, null, null);
            mockCrossCheckScrapeOrchestrator = new Mock<CrossCheckScrapeOrchestrator>(null, null, null);
            mockIndex.Setup(x => x[ScrapeSessionTypes.StatementScrapper]).Returns(mockStatementScrapeOrchestrator.Object);
            mockIndex.Setup(x => x[ScrapeSessionTypes.CrossCheckScrapper]).Returns(mockCrossCheckScrapeOrchestrator.Object);
        }


        [TestMethod]
        public void Given_StatementScrapeType_Then_Initiate_NewStatementScrapeSession()
        {
            //arrange
            var mockScrapeSessionInitiator = new Mock<ScrapeSessionInitiator>(customerBillingCompanyAccountsByIdMock.Object, billingCompanyScrapingUrlQueryMock.Object, mockIndex.Object);
            
            ScrapeSessionInitiator scrapeSessionInitiator = mockScrapeSessionInitiator.Object;
            
            //act
            scrapeSessionInitiator.InitiateNewScrapeSession(new ScrapingObject(customerId, billingCompanyId, ScrapeSessionTypes.StatementScrapper));

            //assert (verify)
            mockScrapeSessionInitiator.Verify();

        }

        [TestMethod]
        public void Given_CrossCheckScrapeType_Then_Initiate_NewStatementScrapeSession()
        {
            //arrange
            var mockScrapeSessionInitiator = new Mock<ScrapeSessionInitiator>(customerBillingCompanyAccountsByIdMock.Object, billingCompanyScrapingUrlQueryMock.Object, mockIndex.Object);
            ScrapeSessionInitiator scrapeSessionInitiator = mockScrapeSessionInitiator.Object;
            
            //act
            scrapeSessionInitiator.InitiateNewScrapeSession(new ScrapingObject(customerId, billingCompanyId, ScrapeSessionTypes.CrossCheckScrapper));
            
            //assert (verify)
            mockScrapeSessionInitiator.Verify();
        }

    }
}
