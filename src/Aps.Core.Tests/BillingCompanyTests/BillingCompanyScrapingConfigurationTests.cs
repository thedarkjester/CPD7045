﻿using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Aps.Fakes;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyScrapingConfigurationTests
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
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();

            container = builder.Build();
        }

        [TestMethod]
        public void Given_An_AddScrapingErrorRetryConfiguration_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
                        BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            //act
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));

            //assert
            Assert.IsTrue(billingCompany.ScrapingErrorRetryConfigurations.Count() == 1);
        }

        [TestMethod]
        public void Given_Multiple_AddScrapingErrorRetryConfiguration_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            //act
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(1, 2));

            //assert
            Assert.IsTrue(billingCompany.ScrapingErrorRetryConfigurations.Count() == 2);
        }

        [ExpectedException(typeof(InvalidOperationException), "Duplicate Error Code Configuration Exists")]
        [TestMethod]
        public void Given_DuplicateErrorCodesOn_AddScrapingErrorRetryConfigurations_When_AddingToTheBillingCompany_ExceptionIsThrown()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            //act
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));

            //assert
            //Exception Expected
        }

        [TestMethod]
        public void Given_A_AddScrapingErrorRetryConfiguration_When_RemovingFromTheBillingCompany_TheItemsInsideDecrease()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));

            var configuration = new ScrapingErrorRetryConfiguration(1, 2);

            billingCompany.AddScrapingErrorRetryConfiguration(configuration);

            //act
            billingCompany.RemoveScrapingErrorRetryConfiguration(configuration);

            //assert
            Assert.IsTrue(billingCompany.ScrapingErrorRetryConfigurations.Count() == 1);
        }
    }
}
