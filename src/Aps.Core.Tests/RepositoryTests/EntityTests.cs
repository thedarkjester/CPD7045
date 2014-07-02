using System;
using Aps.BillingCompanies;
using Aps.BillingCompanies.ValueObjects;
using Aps.Customers;
using Aps.Fakes;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests.RepositoryTests
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void DefaultConstructionOfCustomerGeneratesNonEmptyId()
        {
            //arrange
            BillingCompanyName companyName = new BillingCompanyName("Company A");
            BillingCompanyType companyType = new BillingCompanyType(1);
            BillingCompanyScrapingUrl companyUrl = new BillingCompanyScrapingUrl("https://www.google.com/");

            IContainer container;
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();

            container = builder.Build();

            // act
            var billingCompany = container.Resolve<BillingCompanyFactory>()
                                    .ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            // assert
            Assert.IsTrue(billingCompany.Id != Guid.Empty);
        }
    }
}
