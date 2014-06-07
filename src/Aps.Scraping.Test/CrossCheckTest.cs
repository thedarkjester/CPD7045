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
            //arrange
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, username, password, accountNumber)).Returns(true);
            //act
            bool crossCheck = mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
            //assert
            Assert.IsTrue(crossCheck);
        }

        [TestMethod]
        public void Given_UrlUserCredentialsAndAccountNumber_When_PerformCrossCheck_Then_ReturnFalse()
        {
            //arrange
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, username, password, accountNumber)).Returns(false);
            //act
            bool crossCheck = mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
            //assert
            Assert.IsFalse(crossCheck);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForUrl_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            //arrange
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(null, username, password, accountNumber)).Throws<ArgumentException>();
            url = null;
            //act
            mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForUsername_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            //arrange
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, null, password, accountNumber)).Throws<ArgumentException>();
            username = null;
            //act
            mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForPassword_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            //arrange
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck(url, username, null, accountNumber)).Throws<ArgumentException>();
            password = null;
            //act
            mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_InvalidParameterFoUrl_When_PerformingCrossCheck_Then_ExceptionIsThrown()
        {
            //arrange
            mockCrossCheckWebScraper.Setup(x => x.CrossCheck("Hello", username, password, accountNumber)).Throws<ArgumentException>();
            url = "Hello";
            //act
            bool crossCheck = mockCrossCheckWebScraper.Object.CrossCheck(url, username, password, accountNumber);
        }
    }
}
