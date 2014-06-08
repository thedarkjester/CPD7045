using System;
using System.Linq;
using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Customers.Entities;
using Aps.Customers.ValueObjects;
using Aps.Fakes;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.CustomerTests
{
    [TestClass]
    public class CustomerBillingCompanyAccountTests
    {

        IContainer container;
        private CustomerFirstName customerFirstName;
        private CustomerLastName customerLastName;
        private CustomerEmailAddress customerEmail;
        private CustomerTelephone customerTelephone;
        private CustomerAPSUsername customerAPSUsername;
        private CustomerAPSPassword customerAPSPassword;
        private CustomerStatement customerStatement;
        private CustomerBillingCompanyAccount customerBCAccount;

        [TestInitialize]
        public void Setup()
        {
            Guid guid = new Guid();
            DateTime date = DateTime.Now;
            customerFirstName = new CustomerFirstName("Dexter");
            customerLastName = new CustomerLastName("Morgan");
            customerEmail = new CustomerEmailAddress("DexterMorgan@HBO.com");
            customerTelephone = new CustomerTelephone("27727465885");
            customerAPSUsername = new CustomerAPSUsername("BayHarbourButcher");
            customerAPSPassword = new CustomerAPSPassword("KillTrinity");
            customerStatement = new CustomerStatement(guid, date);

            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>();
            builder.RegisterType<CustomerCreator>().As<CustomerCreator>();

            container = builder.Build(); 
        }

        [TestMethod]
        public void GivenABillingCompanyAccount_WhenAddingToTheCustomer_TheItemsInsideIncrease()
        {
            //arrange
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            var customerBillingCompanyAccount = new CustomerBillingCompanyAccount(Guid.NewGuid(), "Username", "Password", "Trying", "101", 0, DateTime.Now);
            
            //act
            customer.AddCustomerBillingCompanyAccount(customerBillingCompanyAccount);

            //assert
            Assert.IsTrue(customer.CustomerBillingCompanyAccounts.Count() == 1);
        }

        [TestMethod]
        public void GivenMultipleBillingCompanyAccounts_WhenAddingToTheCustomer_TheItemsInsideIncrease()
        {
            //arrange
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);
            var customerBillingCompanyAccount1 = new CustomerBillingCompanyAccount(Guid.NewGuid(), "Username1", "Password", "Trying", "101", 0, DateTime.Now);
            var customerBillingCompanyAccount2 = new CustomerBillingCompanyAccount(Guid.NewGuid(), "Username2", "Password", "Trying", "102", 0, DateTime.Now.AddHours(1));


            //act
            customer.AddCustomerBillingCompanyAccount(customerBillingCompanyAccount1);
            customer.AddCustomerBillingCompanyAccount(customerBillingCompanyAccount2);

            //assert
            Assert.IsTrue(customer.CustomerBillingCompanyAccounts.Count() == 2);
        }

        [TestMethod]
        public void GivenMultipleBillingCompanyAccounts_WhenRemovingFromTheCustomer_TheItemsInsideDecrease()
        {
            //arrange
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            var customerBillingCompanyAccount1 = new CustomerBillingCompanyAccount(Guid.NewGuid(), "Username1", "Password", "Trying", "101", 0, DateTime.Now);
            var customerBillingCompanyAccount2 = new CustomerBillingCompanyAccount(Guid.NewGuid(), "Username2", "Password", "Trying", "102", 0, DateTime.Now.AddHours(1));
            customer.AddCustomerBillingCompanyAccount(customerBillingCompanyAccount1);
            customer.AddCustomerBillingCompanyAccount(customerBillingCompanyAccount2);

            //act
            customer.RemoveCustomerBillingCompanyAccount(customerBillingCompanyAccount1);

            //assert
            Assert.IsTrue(customer.CustomerBillingCompanyAccounts.Count() == 1);
        }

    }
}