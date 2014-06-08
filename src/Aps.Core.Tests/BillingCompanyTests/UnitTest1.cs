using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Aps.Customers;
using Aps.Fakes;
using Aps.Integration;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyDeletedTests
    {
        IContainer container;
        private Guid addedBillingCompanyId = Guid.Empty;

        [TestInitialize]
        public void Setup()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();

            container = builder.Build();

            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();

            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(new BillingCompanyName("test"), new BillingCompanyType(1), new BillingCompanyScrapingUrl("https://www.test.com"));
            addedBillingCompanyId = billingCompany.Id;

            repository.StoreBillingCompany(billingCompany);
        }

        [TestMethod]
        public void Given_A_BillingCompanyId_When_Calling_Delete_BillingCompanyIsDeleted()
        {
            //arrange
            IBillingCompanyRepository repository = container.Resolve<IBillingCompanyRepository>();
            
            //Act
            repository.RemoveBillingCompanyById(addedBillingCompanyId);
            
            //Assert
            Assert.IsTrue(!repository.GetAllBillingCompanies().Any());
        }

        [TestMethod]
        public void Given_A_BillingCompanyId_When_Calling_Delete_BillingCompanyDeleteEventIsPublished()
        {

        }
    }
}
