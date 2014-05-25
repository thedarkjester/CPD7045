using System;
using Aps.Customer;
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
            builder.RegisterType<CustomerRepositoryFake>().As<CustomerRepositoryFake>();

            container = builder.Build();

            // act
            var customer = container.Resolve<CustomerRepositoryFake>().GetNewCustomer();

            // assert
            Assert.IsTrue(customer.Id != Guid.Empty);
        }
    }
}
