using System;
using Aps.Scheduling.ApplicationService;
using Aps.Scheduling.ApplicationService.Interpreters;
using Aps.Scheduling.ApplicationService.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aps.Scheduling.ApplicationService;
using System.Collections.Generic;
using Aps.Scheduling.ApplicationService.Validation;

namespace Aps.Shared.Tests.ValidatorTests
{
    [TestClass]
    public class ScrapeSessionDataValidatorTests
    {
        private string xml;
        private IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair;

        [TestInitialize]
        public void Setup()
        {
            xml =
                @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><text>Error Code</text><value>2</value></datapair></scrape-session>";
            ScrapeSessionXMLToDataPairConverter converter = new ScrapeSessionXMLToDataPairConverter();
            interpretedScrapeSessionDataPair = converter.ConvertXmlToScrapeSessionDataPairs(xml);
        }

        [TestMethod]
        [ExpectedException(typeof(ScrapeValidationException))]
        public void GivenScrapeSessionDataPairsWithInvalidCredentials_WhenValidationisRequested_ScrapeInvalidCredentialsExceptionShouldBeReturned()
        {
            //Arrange
            ScrapeSessionDataValidator scrapeSessionDataValidator = new ScrapeSessionDataValidator();
            //Act
            scrapeSessionDataValidator.validateScrapeData(interpretedScrapeSessionDataPair);

        }
    }
}
