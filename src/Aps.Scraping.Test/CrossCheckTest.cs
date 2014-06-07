using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Aps.Scraping.Scrapers;

namespace Aps.Scraping.Test
{
    [TestClass]
    public class CrossCheckTest
    {
        Mock<ICrossCheckScraper> mockCrossCheckWebScraper;
        string url;
        string username;
        string password;
        string accountNumber;
        [TestInitialize]
        public void Setup()
        {
            mockCrossCheckWebScraper = new Mock<ICrossCheckScraper>();

            url = "http://www.telkom.co.za";
            username = "dave";
            password = "d@v3!";
            accountNumber = "1234566";
        }

        [TestMethod]
        public void Given_UrlUserCredentialsAndAccountNumber_When_PerformCrossCheck_Then_ReturnTrue()
        {
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, username, password, accountNumber)).Returns(true);
            bool crossCheck = mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
            Assert.IsTrue(crossCheck);
        }

        [TestMethod]
        public void Given_UrlUserCredentialsAndAccountNumber_When_PerformCrossCheck_Then_ReturnFalse()
        {
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, username, password, accountNumber)).Returns(false);

            bool crossCheck = mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
            Assert.IsFalse(crossCheck);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForUrl_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(null, username, password, accountNumber)).Throws<ArgumentException>();
            url = null;
            mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForUsername_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, null, password, accountNumber)).Throws<ArgumentException>();
            username = null;
            mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForPassword_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, username, null, accountNumber)).Throws<ArgumentException>();
            password = null;
            mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_InvalidParameterFoUrl_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck("Hello", username, password, accountNumber)).Throws<ArgumentException>();
            url = "Hello";
            bool crossCheck = mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }
    }
}
