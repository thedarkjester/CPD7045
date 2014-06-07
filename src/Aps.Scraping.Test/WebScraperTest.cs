using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Aps.Scraping.Scrapers;

namespace Aps.Scraping.Test
{
    [TestClass]
    public class WebScraperTest
    {
        Mock<IWebScraper> mockWebScraper;
        string url;
        string username;
        string password;

        [TestInitialize]
        public void Setup()
        {
            mockWebScraper = new Mock<IWebScraper>();

            url ="http://www.telkom.co.za";
            username = "dave";
            password = "d@v3!";
        }

        [TestMethod]
        public void Given_UrlAndUserCredentials_When_PerformingScrape_Then_ReturnXmlData()
        {
            mockWebScraper.Setup(x => x.Scrape(url, username, password)).Returns("<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=001><text>Account no</text><value>53844946068883</value></datapair><datapair id=002><text>Service ref</text><value>0117838898</value></datapair><datapair id=003><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=004><text>Payment</text><value>R513.00</value></datapair><datapair id=005><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>");
            mockWebScraper.Object.Scrape(url, username, password);

            mockWebScraper.Verify();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForUrl_When_PerformingScrape_Then_ExceptionIsThrown()
        {
            mockWebScraper.Setup(x => x.Scrape(null, username, password)).Throws<ArgumentException>();
            url = null;
            mockWebScraper.Object.Scrape(url, username, password);           
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForUsername_When_PerformingScrape_Then_ExceptionIsThrown()
        {
            mockWebScraper.Setup(x => x.Scrape(url, null, password)).Throws<ArgumentException>();
            username = null;
            mockWebScraper.Object.Scrape(url, username, password);   
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_NullParameterForPassword_When_PerformingScrape_Then_ExceptionIsThrown()
        {
            mockWebScraper.Setup(x => x.Scrape(url, username, null)).Throws<ArgumentException>();
            password = null;
            mockWebScraper.Object.Scrape(url, username, password);          
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_InvalidParameterForUrl_When_PerformingScrape_Then_ExceptionIsThrown()
        {
            mockWebScraper.Setup(x => x.Scrape("Hello", username, password)).Throws<ArgumentException>();
            url = "Hello";
            mockWebScraper.Object.Scrape(url, username, password);           
        }
    }
}
