using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyBillingCompanyUrlTests
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

        [TestMethod]
        public void GivenABillingCompanyUrl_SettingItsValue_ChangesItsIntance()
        {
            //arrange
            url = "https://www.google.com";
            var billingCompanyUrl = new BillingCompanyUrl(url);

            //act

            var billingCompanyUrl2 = billingCompanyUrl.ChangeUrl("https://www.google.com");

            //assert
            Assert.IsTrue(billingCompanyUrl2 != billingCompanyUrl);
        }
    }
}
