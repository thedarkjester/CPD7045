using System;
using System.Collections.Generic;
using Aps.AccountStatements.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests.AccountStatementTests
{
    [TestClass]
    public class BillingCompanyDetailsConstructionTests
    {
        private string billingCompanyName;
        private Guid billingCompanyId;

        [TestInitialize]
        public void Setup()
        {
            billingCompanyName = "test";
            billingCompanyId = Guid.NewGuid();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Empty_Parameter_For_BillingCompanyName_WhenConstructingBillingCompanyDetails_Exception_IsThrown()
        {
            //arrange
            billingCompanyName = "";

            //act
            var billingCompanyDetails = new BillingCompanyDetails(billingCompanyId, billingCompanyName);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_Null_Parameter_For_BillingCompanyName_WhenConstructingBillingCompanyDetails_Exception_IsThrown()
        {
            //arrange
            billingCompanyName = null;

            //act
            var billingCompanyDetails = new BillingCompanyDetails(billingCompanyId, billingCompanyName);

            //assert
            //Exception expected
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Empty_Parameter_For_BillingCompanyId_WhenConstructingBillingCompanyDetails_Exception_IsThrown()
        {
            //arrange
            billingCompanyId = Guid.Empty;

            //act
            var billingCompanyDetails = new BillingCompanyDetails(billingCompanyId, billingCompanyName);

            //assert
            //Exception expected
        }

        [TestMethod]
        public void Given_Valid_Parameters_For_WhenConstructingBillingCompanyDetails_SettingIsDoneCorrectly()
        {
            //arrange
            //as per testinitialize

            //act
            var billingCompanyDetails = new BillingCompanyDetails(billingCompanyId, billingCompanyName);

            //assert
            Assert.IsTrue(billingCompanyDetails.BillingCompanyId == billingCompanyId);
            Assert.IsTrue(billingCompanyDetails.CompanyName == billingCompanyName);
        }
    }
}