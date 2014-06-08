using System;
using System.Linq;
using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Customers.ValueObjects;
using Aps.Fakes;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.CustomerTests
{
    [TestClass]
    class CustomerAggregateTests
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
        public void WhenCreatingACustomerTheDefaultIdMustNotBeEmpty()
        {
            // arrange ( repository and depenency injection done )

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName, 
                                                                                        customerEmail, customerTelephone, 
                                                                                        customerAPSUsername, customerAPSPassword);

            // assert
            Assert.IsTrue(customer.Id != Guid.Empty);
        }

        [TestMethod]
        public void WhenCreatingANewCustomerTheListOfBillingCompanyAccountsShouldBeEmptyAndCountZero()
        {
            // arrange ( repository and depenency injection done )

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                                        customerEmail, customerTelephone,
                                                                                        customerAPSUsername, customerAPSPassword);

            // assert
            Assert.IsTrue(customer.CustomerBillingCompanyAccounts != null);
            Assert.IsTrue(!customer.CustomerBillingCompanyAccounts.Any());
        }

        [ExpectedException(typeof (ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheFirstNameShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            customerFirstName = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheLastNameShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            customerLastName = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheEmailAddressShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            customerEmail = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheTelephoneShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            customerTelephone = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheAPSUsernameShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            customerAPSUsername = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheAPSPasswordShouldNotNull()
        {
            // arrange ( repository and depenency injection done )
            customerAPSPassword = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WhenCreatingANewCustomerTheStatementShouldbeEmpty()
        {
            // arrange ( repository and depenency injection done )
            customerAPSPassword = null;

            // act
            Customer customer = container.Resolve<ICustomerRepository>().GetNewCustomer(customerFirstName, customerLastName,
                                                                            customerEmail, customerTelephone,
                                                                            customerAPSUsername, customerAPSPassword);

            // assert
            // expected exception
        }
    }
}
