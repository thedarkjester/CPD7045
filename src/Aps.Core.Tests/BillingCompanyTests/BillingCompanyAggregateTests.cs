using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyAggregateTests
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
            builder.RegisterType<BillingCompanyRepositoryFake>().As<BillingCompanyRepositoryFake>();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>();

            container = builder.Build(); 
        }

        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheDefaultIdMustNotBeEmpty()
        {
            // arrange ( repository and depenency injection done )

            // act
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().GetNewBillingCompany(companyName, companyType, companyUrl);

            // assert
            Assert.IsTrue(billingCompany.Id != Guid.Empty);
        }

        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheListOfOpenClosedWindowsShouldBeEmptyAndCountZero()
        {
            // arrange ( repository and depenency injection done )

            // act
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().GetNewBillingCompany(companyName, companyType, companyUrl);

            // assert
            Assert.IsTrue(billingCompany.OpenClosedWindows != null);
            Assert.IsTrue(!billingCompany.OpenClosedWindows.Any());
        }

        [ExpectedException(typeof (ArgumentNullException))]
        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheNameShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            companyName = null;

            // act
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().
                GetNewBillingCompany(companyName, companyType, companyUrl);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheCompanyTypeShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            companyType = null;

            // act
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().
                GetNewBillingCompany(companyName, companyType, companyUrl);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheUrlShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            companyUrl = null;

            // act
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().
                GetNewBillingCompany(companyName, companyType, companyUrl);

            // assert
            // expected exception
        }

        [TestMethod]
        public void Given_An_AddScrapingErrorRetryConfiguration_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().GetNewBillingCompany(companyName, companyType, companyUrl);

            //act
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0,2));

            //assert
            Assert.IsTrue(billingCompany.ScrapingErrorRetryConfigurations.Count() == 1);
        }

        [TestMethod]
        public void Given_Multiple_AddScrapingErrorRetryConfiguration_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().GetNewBillingCompany(companyName, companyType, companyUrl);

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
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().GetNewBillingCompany(companyName, companyType, companyUrl);

            //act
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));
            billingCompany.AddScrapingErrorRetryConfiguration(new ScrapingErrorRetryConfiguration(0, 2));

            //assert
            //Exception Expected
        }
    }
}
