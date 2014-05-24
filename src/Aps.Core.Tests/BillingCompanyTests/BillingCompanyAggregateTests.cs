using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Customer;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Core.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyAggregateTests
    {
        IContainer container;

        [TestInitialize]
        public void Setup()
        {
            //arrange
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<BillingCompanyRepository>().As<BillingCompanyRepository>();

            container = builder.Build(); 
        }

        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheDefaultIdMustNotBeEmpty()
        {
            // arrange ( repository and depenency injection done )

            // act
            BillingCompanies.Aggregates.BillingCompany billingCompany = container.Resolve<BillingCompanyRepository>().GetNewBillingCompany();

            // assert
            Assert.IsTrue(billingCompany.Id != Guid.Empty);
        }

        [TestMethod]
        public void WhenConstructingANewBillingCompanyTheListOfOpenClosedWindowsShouldBeEmptyAndCountZero()
        {
            // arrange ( repository and depenency injection done )

            // act
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepository>().GetNewBillingCompany();

            // assert
            Assert.IsTrue(billingCompany.OpenClosedWindows != null);
            Assert.IsTrue(!billingCompany.OpenClosedWindows.Any());
        }

    }
}
