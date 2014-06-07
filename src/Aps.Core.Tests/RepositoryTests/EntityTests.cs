using System;
using Aps.BillingCompanies;
using Aps.Customers;
using Aps.Fakes;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.RepositoryTests
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void DefaultConstructionOfCustomerGeneratesNonEmptyId()
        {
            //arrange

            IContainer container;
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>();

            container = builder.Build();

            // act
            //var customer = container.Resolve<ICustomerRepository>().GetNewCustomer(;

            // assert
            //Assert.IsTrue(customer.Id != Guid.Empty);
        }
    }
}
