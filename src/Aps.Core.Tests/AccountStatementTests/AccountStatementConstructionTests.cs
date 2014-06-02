using System;
using System.Collections.Generic;
using Aps.AccountStatements.Entities;
using Aps.AccountStatements.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.AccountStatementTests
{
    [TestClass]
    public class AccountStatementConstructionTests
    {
        private List<AccountStatementTransaction> statementTransactions;
        private BillingCompanyDetails billingCompanyDetails;
        private CustomerDetails customerDetails;
        private StatementDate statementDate;

        [TestInitialize]
        public void Setup()
        {
            statementTransactions = new List<AccountStatementTransaction>();
            billingCompanyDetails = new BillingCompanyDetails(Guid.NewGuid(),"test");
            customerDetails = new CustomerDetails(Guid.NewGuid(),"customername");
            statementDate = new StatementDate(DateTime.UtcNow);

            statementTransactions.Add(new AccountStatementTransaction(10M,0M,"test transaction",100M));
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_Null_Parameter_For_StatementTransactions_WhenConstructingAccountStatement_Exception_IsThrown()
        {
            //arrange
            statementTransactions = null;

            //act
            var accountStatement = new AccountStatement(customerDetails,billingCompanyDetails,statementDate,statementTransactions);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_EmptyList_Parameter_For_StatementTransactions_WhenConstructingAccountStatement_Exception_IsThrown()
        {
            //arrange
            statementTransactions = new List<AccountStatementTransaction>();

            //act
            var accountStatement = new AccountStatement(customerDetails, billingCompanyDetails, statementDate, statementTransactions);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_Null_Parameter_For_CustomerDetails_WhenConstructingAccountStatement_Exception_IsThrown()
        {
            //arrange
            customerDetails = null;

            //act
            var accountStatement = new AccountStatement(customerDetails, billingCompanyDetails, statementDate, statementTransactions);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_Null_Parameter_For_BillingDetails_WhenConstructingAccountStatement_Exception_IsThrown()
        {
            //arrange
            billingCompanyDetails = null;

            //act
            var accountStatement = new AccountStatement(customerDetails, billingCompanyDetails, statementDate, statementTransactions);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_Null_Parameter_For_StatementDate_WhenConstructingAccountStatement_Exception_IsThrown()
        {
            //arrange
            statementDate = null;

            //act
            var accountStatement = new AccountStatement(customerDetails, billingCompanyDetails, statementDate, statementTransactions);

            //assert
            //Exception expected
        }
    }
}
