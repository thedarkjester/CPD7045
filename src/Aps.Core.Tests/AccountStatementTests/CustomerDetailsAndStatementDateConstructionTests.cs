using System;
using Aps.AccountStatements.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests.AccountStatementTests
{
    [TestClass]
    public class CustomerDetailsAndStatementDateConstructionTests
    {
        private string customerName;
        private Guid customerId;

        [TestInitialize]
        public void Setup()
        {
            customerName = "test";
            customerId = Guid.NewGuid();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Empty_Parameter_For_CustomerDetailsName_WhenConstructingCustomerDetails_Exception_IsThrown()
        {
            //arrange
            customerName = "";

            //act
            var customerDetails = new CustomerDetails(customerId, customerName);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_Null_Parameter_For_CustomerDetailsName_WhenConstructingCustomerDetails_Exception_IsThrown()
        {
            //arrange
            customerName = null;

            //act
            var customerDetails = new CustomerDetails(customerId, customerName);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Empty_Parameter_For_CustomerDetailsId_WhenConstructingCustomerDetails_Exception_IsThrown()
        {
            //arrange
            customerId = Guid.Empty;

            //act
            var customerDetails = new CustomerDetails(customerId, customerName);

            //assert
            //Exception expected
        }

        [TestMethod]
        public void Given_Valid_Parameters_For_WhenConstructingCustomerDetails_SettingIsDoneCorrectly()
        {
            //arrange
            //as per testinitialize

            //act
            var customerDetails = new CustomerDetails(customerId, customerName);

            //assert
            Assert.IsTrue(customerDetails.CustomerId == customerId);
            Assert.IsTrue(customerDetails.CustomerName == customerName);
        }

        [TestMethod]
        public void Given_Parameters_For_StatementDate_SettingIsDoneCorrectly()
        {
            //arrange
            DateTime statementDate = DateTime.UtcNow;

            //act
            var statementDateObject = new StatementDate(statementDate);

            //assert
            Assert.IsTrue(statementDateObject.DateOfStatement == statementDate);
        }
    }
}