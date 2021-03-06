﻿using Aps.Scheduling.ApplicationService;
using Aps.Scheduling.ApplicationService.Interpreters;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Domain.Tests.InterpreterTests
{
    [TestClass]
    public class ScrapeSessionXMLToDataPairConverterTests
    {
        private string xml;

        [TestInitialize]
        public void Setup()
        {
            xml =
                @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><text>Account no</text><value>53844946068883</value></datapair><datapair id=""002""><text>Service ref</text><value>0117838898</value></datapair><datapair id=""003""><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=""004""><text>Payment</text><value>R513.00</value></datapair><datapair id=""005""><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>";
        }

        [TestMethod]
        public void GivenValidXmlWith10DataPairs_WhenConvertingToDataPairs_ScrapeSessionDataPairsShouldBeReturned()
        {
            //Arrange
            ScrapeSessionXMLToDataPairConverter converter = new ScrapeSessionXMLToDataPairConverter();
            xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><text>Account no</text><value>53844946068883</value></datapair><datapair id=""002""><text>Service ref</text><value>0117838898</value></datapair><datapair id=""003""><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=""004""><text>Payment</text><value>R513.00</value></datapair><datapair id=""005""><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>";
            //Act
            var dataPairs = converter.ConvertXmlToScrapeSessionDataPairs(xml);
            //Assert
            Assert.AreEqual(5, dataPairs.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(DataScraperException), "There were no data pairs in the xml")]
        public void GivenValidXmlWithNoDataPairs_WhenConvertingToDataPairs_ScrapeNoDataPairsFoundExceptionShouldBeReturned()
        {
            //Arrange
            xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time></scrape-session>";
            ScrapeSessionXMLToDataPairConverter interpreter = new ScrapeSessionXMLToDataPairConverter();
            //Act
            var dataPairs = interpreter.ConvertXmlToScrapeSessionDataPairs(xml);
            //Assert
            //There is an expected exception thrown

        }

        [TestMethod]
        public void GivenValidXmlWith10DataPairsAnd1WithoutName_WhenConvertingToDataPairs_ScrapeSessionDataPairsShouldBeReturned()
        {
            //Arrange
            xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><value>53844946068883</value></datapair><datapair id=""002""><text>Service ref</text><value>0117838898</value></datapair><datapair id=""003""><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=""004""><text>Payment</text><value>R513.00</value></datapair><datapair id=""005""><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>";
            ScrapeSessionXMLToDataPairConverter interpreter = new ScrapeSessionXMLToDataPairConverter();
            //Act
            var dataPairs = interpreter.ConvertXmlToScrapeSessionDataPairs(xml);
            //Assert
            Assert.AreEqual(4, dataPairs.Count);

        }
    }


}
