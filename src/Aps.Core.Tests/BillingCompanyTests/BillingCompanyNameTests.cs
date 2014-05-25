using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyNameTests
    {
        private string name = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            name = "";

            //act

            BillingCompanyName companyName = new BillingCompanyName(name);

            //assert
            //Exception Expected
        }
    }
}