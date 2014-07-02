using System;
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
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();

            container = builder.Build(); 
        }

        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheDefaultIdMustNotBeEmpty()
        {
            // arrange ( repository and depenency injection done )

            // act
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            // assert
            Assert.IsTrue(billingCompany.Id != Guid.Empty);
        }

        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheListOfOpenClosedWindowsShouldBeEmptyAndCountZero()
        {
            // arrange ( repository and depenency injection done )

            // act
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            // assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows != null);
            Assert.IsTrue(!billingCompany.OpenClosedScrapingWindows.Any());
        }

        [ExpectedException(typeof (ArgumentNullException))]
        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheNameShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            companyName = null;

            // act
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

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
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

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
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            // assert
            // expected exception
        }

    }
}
