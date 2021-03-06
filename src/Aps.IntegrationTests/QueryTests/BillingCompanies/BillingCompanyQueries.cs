﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.ValueObjects;
using Aps.Fakes;
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
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>().SingleInstance();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>().SingleInstance();
            builder.RegisterType<BillingCompanyByIdQuery>().As<BillingCompanyByIdQuery>();
            builder.RegisterType<BillingCompanyBillingLifeCycleByCompanyIdQuery>().As<BillingCompanyBillingLifeCycleByCompanyIdQuery>();
            builder.RegisterType<BillingCompanyScrapingUrlQuery>().As<BillingCompanyScrapingUrlQuery>();
            builder.RegisterType<AllBillingCompaniesQuery>().As<AllBillingCompaniesQuery>();
            builder.RegisterType<BillingCompanyOpenClosedWindowsQuery>().As<BillingCompanyOpenClosedWindowsQuery>();
            builder.RegisterType<ScrapingErrorRetryConfigurationQuery>().As<ScrapingErrorRetryConfigurationQuery>();
            builder.RegisterType<BillingCompanyScrapingLoadManagementConfigurationQuery>().As<BillingCompanyScrapingLoadManagementConfigurationQuery>();

            container = builder.Build();
        }

        //NOTE : THE QUERY TESTS ALSO IN EFFECT ARE THE SETTER TESTS, ELSE THE COMPARISON CHECKS WOULD HAVE FAILED!
        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompany_DtoReturns_NullIfNotFound()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();

            //act
            var billingCompany = repository.GetBillingCompanyById(Guid.NewGuid());

            //assert
            Assert.IsTrue(billingCompany == null);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompanyByMissingId_DtoReturns_Null()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();

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
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

            repository.StoreBillingCompany(newBillingCompany);

            //act
            BillingCompanyScrapingUrlQuery query = container.Resolve<BillingCompanyScrapingUrlQuery>();

            BillingCompanyScrapingUrlDto billingCompany = query.GetBillingCompanyScrapingUrlById(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompany != null);
            Assert.IsTrue(billingCompany.Url == newBillingCompany.BillingCompanyScrapingUrl.ScrapingUrl);
            Assert.IsTrue(billingCompany.Id == newBillingCompany.Id);
            Assert.IsTrue(billingCompany.CrossCheckScrapeEnabled);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingBillingCompanyById_DtoReturns_CorrectData()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

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
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

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
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

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
            Assert.IsTrue(!billingCompany.OpenClosedScrapingWindows.Any());
            Assert.IsTrue(!billingCompany.ScrapingErrorRetryConfigurations.Any());
        }

        [TestMethod]
        public void WhenQueryingAll_BillingCompanies_AllBillingCompanyDtosAreReturned()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));
            var newBillingCompany2 = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));
            var newBillingCompany3 = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

            repository.StoreBillingCompany(newBillingCompany);
            repository.StoreBillingCompany(newBillingCompany2);
            repository.StoreBillingCompany(newBillingCompany3);

            //act
            AllBillingCompaniesQuery query = container.Resolve<AllBillingCompaniesQuery>();

            IEnumerable<BillingCompanyDto> billingCompanies = query.GetAllBillingCompanies();

            //assert
            Assert.IsTrue(billingCompanies.Count() == 3);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingOpenClosedWindows_RepositoryReturns_AllWindows()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

            newBillingCompany.AddOpenClosedScrapingWindow(new OpenClosedScrapingWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2));
            newBillingCompany.AddOpenClosedScrapingWindow(new OpenClosedScrapingWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), false, 2));
            newBillingCompany.AddOpenClosedScrapingWindow(new OpenClosedScrapingWindow(DateTime.Now.AddHours(5), DateTime.Now.AddHours(6), false, 2));

            repository.StoreBillingCompany(newBillingCompany);

            //act
            BillingCompanyOpenClosedWindowsQuery query = container.Resolve<BillingCompanyOpenClosedWindowsQuery>();

            IEnumerable<OpenClosedWindowDto> billingCompanies = query.GetAllOpenClosedWindows(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompanies.Count() == 3);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingRetryConfigurations_RepositoryReturns_AllConfigurations()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

            newBillingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(1, 1));
            newBillingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(2, 1));
            newBillingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(3, 1));

            repository.StoreBillingCompany(newBillingCompany);

            //act
            ScrapingErrorRetryConfigurationQuery query = container.Resolve<ScrapingErrorRetryConfigurationQuery>();

            IEnumerable<ScrapingErrorRetryConfigurationDto> billingCompanies = query.GetAllScrapingErrorRetryConfigurations(newBillingCompany.Id);

            //assert
            Assert.IsTrue(billingCompanies.Count() == 3);
        }

        [TestMethod]
        public void Given_A_BillingCompany_When_QueryingRetryConfigurations_RepositoryReturns_ScrapingLoadManagementConfiguration()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            var newBillingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl, new BillingCompanyCrossCheckScrapeEnabled(true));

            newBillingCompany.SetScrapingLoadManagementConfiguration(new ScrapingLoadManagementConfiguration(5));

            repository.StoreBillingCompany(newBillingCompany);

            //act
            BillingCompanyScrapingLoadManagementConfigurationQuery query = container.Resolve<BillingCompanyScrapingLoadManagementConfigurationQuery>();

            BillingCompanyScrapingLoadManagementConfigurationDto companyScrapingLoadManagementConfiguration = query.GetBillingCompanyScrapingLoadManagementConfigurationById(newBillingCompany.Id);

            //assert
            Assert.IsTrue(companyScrapingLoadManagementConfiguration.ConcurrentScrapes == 5);
        }
    }
}
