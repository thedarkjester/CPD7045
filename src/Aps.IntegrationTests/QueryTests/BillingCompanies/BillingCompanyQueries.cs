using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.ValueObjects;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.IntegrationTests.QueryTests.BillingCompanies
{
    [TestClass]
    public class BillingCompanyQueries
    {
        IContainer container;
        private BillingCompanyName companyName;
        private BillingCompanyType companyType;
        private BillingCompanyScrapingUrl companyUrl;

        [TestInitialize]
        public void Setup()
        {
            companyName = new BillingCompanyName("Company A");
            companyType = new BillingCompanyType(1);
            companyUrl = new BillingCompanyScrapingUrl("https://www.google.com/");

            //arrange
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<BillingCompanyRepositoryFake>().SingleInstance();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>().SingleInstance();
            builder.RegisterType<BillingCompanyByIdQuery>().As<BillingCompanyByIdQuery>();
            builder.RegisterType<BillingCompanyBillingLifeCycleByCompanyIdQuery>().As<BillingCompanyBillingLifeCycleByCompanyIdQuery>();
            builder.RegisterType<BillingCompanyScrapingUrlQuery>().As<BillingCompanyScrapingUrlQuery>();

            container = builder.Build();
        }

        //NOTE : THE QUERY TESTS ALSO IN EFFECT ARE THE SETTER TESTS, ELSE THE COMPARISON CHECKS WOULD HAVE FAILED!
        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompany_DtoReturns_NullIfNotFound()
        {
            //arrange
            BillingCompanyRepositoryFake repository = container.Resolve<BillingCompanyRepositoryFake>();

            //act
            var billingCompany = repository.GetBillingCompanyById(Guid.NewGuid());

            //assert
            Assert.IsTrue(billingCompany == null);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompanyByMissingId_DtoReturns_Null()
        {
            //arrange
            BillingCompanyRepositoryFake repository = container.Resolve<BillingCompanyRepositoryFake>();

            //act
            BillingCompanyByIdQuery query = container.Resolve<BillingCompanyByIdQuery>();

            BillingCompanyDto billingCompany = query.GetBillingCompanyById(Guid.NewGuid());

            //assert
            Assert.IsTrue(billingCompany == null);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompanyScrapingUrlById_DtoReturns_CorrectData()
        {
            //arrange
            BillingCompanyRepositoryFake repository = container.Resolve<BillingCompanyRepositoryFake>();

            var newBillingCompany = repository.GetNewBillingCompany(companyName, companyType,companyUrl);

            repository.StoreBillingCompany(newBillingCompany);

            //act
            BillingCompanyScrapingUrlQuery query = container.Resolve<BillingCompanyScrapingUrlQuery>();

            BillingCompanyScrapingUrlDto billingCompany = query.GetBillingCompanyScrapingUrlById(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompany != null);
            Assert.IsTrue(billingCompany.Url == newBillingCompany.BillingCompanyScrapingUrl.ScrapingUrl);
            Assert.IsTrue(billingCompany.Id == newBillingCompany.Id);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompanyById_DtoReturns_CorrectData()
        {
            //arrange
            BillingCompanyRepositoryFake repository = container.Resolve<BillingCompanyRepositoryFake>();

            var newBillingCompany = repository.GetNewBillingCompany(companyName, companyType, companyUrl);

            repository.StoreBillingCompany(newBillingCompany);

            //act
            BillingCompanyByIdQuery query = container.Resolve<BillingCompanyByIdQuery>();

            BillingCompanyDto billingCompany = query.GetBillingCompanyById(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompany != null);
            Assert.IsTrue(billingCompany.Name == newBillingCompany.BillingCompanyName.Name);
            Assert.IsTrue(billingCompany.Id == newBillingCompany.Id);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompanyLifeCycleById_DtoReturns_CorrectData()
        {
            //arrange
            BillingCompanyRepositoryFake repository = container.Resolve<BillingCompanyRepositoryFake>();

            var newBillingCompany = repository.GetNewBillingCompany(companyName, companyType, companyUrl);

            newBillingCompany.SetBillingCompanyName(new BillingCompanyName("Company A"));
            newBillingCompany.SetBillingLifeCycle(new BillingLifeCycle(1, 2, 3));

            repository.StoreBillingCompany(newBillingCompany);

            //act
            BillingCompanyBillingLifeCycleByCompanyIdQuery query = container.Resolve<BillingCompanyBillingLifeCycleByCompanyIdQuery>();

            BillingCompanyBillingLifeCycleDto billingCompany = query.GetBillingCompanyBillingLifeCycleByCompanyId(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompany != null);
            Assert.IsTrue(newBillingCompany.BillingLifeCycle.DaysPerBillingCycle == billingCompany.DaysPerBillingCycle);
            Assert.IsTrue(newBillingCompany.BillingLifeCycle.LeadTimeInterval == billingCompany.LeadTimeInterval);
            Assert.IsTrue(newBillingCompany.BillingLifeCycle.RetryInterval == billingCompany.RetryInterval);
            Assert.IsTrue(billingCompany.Id == newBillingCompany.Id);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompany_RepositoryReturns_CorrectNonListData()
        {
            //arrange
            BillingCompanyRepositoryFake repository = container.Resolve<BillingCompanyRepositoryFake>();

            var newBillingCompany = repository.GetNewBillingCompany(companyName, companyType, companyUrl);

            newBillingCompany.SetBillingCompanyName(new BillingCompanyName("Company A"));
            newBillingCompany.SetBillingCompanyType(new BillingCompanyType(1));
            newBillingCompany.SetBillingCompanyUrl(new BillingCompanyScrapingUrl("https://www.google.com/"));
            newBillingCompany.SetBillingLifeCycle(new BillingLifeCycle(1, 2, 3));

            repository.StoreBillingCompany(newBillingCompany);

            //act
            var billingCompany = repository.GetBillingCompanyById(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompany != null);
            Assert.IsTrue(billingCompany.Id == newBillingCompany.Id);
            Assert.IsTrue(billingCompany.BillingCompanyName.Name == "Company A");
            Assert.IsTrue(billingCompany.BillingCompanyType.TypeCode == 1);
            Assert.IsTrue(billingCompany.BillingCompanyScrapingUrl.ScrapingUrl == "https://www.google.com/");
            Assert.IsTrue(billingCompany.BillingLifeCycle.DaysPerBillingCycle == 1);
            Assert.IsTrue(billingCompany.BillingLifeCycle.LeadTimeInterval == 2);
            Assert.IsTrue(billingCompany.BillingLifeCycle.RetryInterval == 3);
            Assert.IsTrue(!billingCompany.OpenClosedWindows.Any());
            Assert.IsTrue(!billingCompany.ScrapingErrorRetryConfigurations.Any());
        }
    }
}
