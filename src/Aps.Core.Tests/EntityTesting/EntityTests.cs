using System;
using Aps.Customer;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Core.Tests.EntityTesting
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
            builder.RegisterType<CustomerRepository>().As<CustomerRepository>();

            container = builder.Build();

            // act
            var customer = container.Resolve<CustomerRepository>().GetNewCustomer();

            // assert
            Assert.IsTrue(customer.Id != Guid.Empty);
        }
    }
}
