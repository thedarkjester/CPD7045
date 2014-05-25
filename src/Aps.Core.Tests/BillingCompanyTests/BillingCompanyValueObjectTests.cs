using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyValueObjectTests
    {
        private string url;

        [TestInitialize]
        public void Setup()
        {
            url = "http://www.site.com";
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GivenAnEmptyString_WhenConstructingABillingCompanyUrl_ExceptionIsThrown()
        {
            //arrange
            url = "";

            //act
            var billingCompanyUrl = new BillingCompanyUrl(url);

            //assert
            //exception attribute
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GivenAUrlWithHttpAndNotHttps_WhenConstructingABillingCompanyUrl_ExceptionIsThrown()
        {
            //arrange
            url = "http://www.google.com";

            //act
            var billingCompanyUrl = new BillingCompanyUrl(url);

            //assert
            //exception attribute
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GivenUnmakeableUrl_WhenConstructingABillingCompanyUrl_ExceptionIsThrown()
        {
            //arrange
            url = "s.com";

            //act
            var billingCompanyUrl = new BillingCompanyUrl(url);

            //assert
            //exception attribute
        }

        [TestMethod]
        public void GivenAvalidUrl_WhenConstructingABillingCompanyUrl_NoExceptionIsThrown()
        {
            //arrange
            url = "https://www.google.com";

            //act
            var billingCompanyUrl = new BillingCompanyUrl(url);

            //assert
            Assert.IsTrue(billingCompanyUrl.ToString() == "https://www.google.com");
        }
    }
}
