using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Aps.Scraping.WebScrapers;

namespace Aps.Scraping.Test
{
    [TestClass]
    public class WebScraperTest
    {
        Mock<WebScraperFake> mockWebScraper;
        string url;
        string username;
        string password;

        [TestInitialize]
        public void Setup()
        {
            mockWebScraper = new Mock<WebScraperFake>();

            url ="http://www.telkom.co.za";
            username = "dave";
            password = "d@v3!";
        }

        [TestMethod]
        public void Given_UrlAndUserCredentials_Then_PerformScrape()
        {
            mockWebScraper.Object.Scrape(url, username, password);

            mockWebScraper.Verify();

        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Null_Parameter_For_Url_WhenInitiatingAScrape_Exception_IsThrown()
        {
            url = null;
            mockWebScraper.Object.Scrape(url, username, password);
           
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Null_Parameter_For_Username_WhenInitiatingAScrape_Exception_IsThrown()
        {
            username = null;
            mockWebScraper.Object.Scrape(url, username, password);

   
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Null_Parameter_For_Password_WhenInitiatingAScrape_Exception_IsThrown()
        {
            password = null;
            mockWebScraper.Object.Scrape(url, username, password);

          
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_Invalid_Parameter_For_Url_WhenInitiatingAScrape_Exception_IsThrown()
        {
            url = "Hello";
            mockWebScraper.Object.Scrape(url, username, password);

           
        }
    }
}
