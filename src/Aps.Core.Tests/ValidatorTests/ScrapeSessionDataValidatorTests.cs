using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aps.Core;
using Aps.Core.Validation;
using System.Collections.Generic;

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
            xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><text>Error Code</text><value>2</value></datapair></scrape-session>";
            ScrapeSessionXMLToDataPairConverter converter = new ScrapeSessionXMLToDataPairConverter();
            interpretedScrapeSessionDataPair = converter.ConvertXmlToScrapeSessionDataPairs(xml);
        }

        [TestMethod]
        [ExpectedException(typeof(DataScraperException))]
        public void GivenScrapeSessionDataPairsWithInvalidCredentials_WhenValidationisRequested_ScrapeInvalidCredentialsExceptionShouldBeReturned()
        {
            //Arrange
            List<IValidator> validatorList = new List<IValidator>();
            validatorList.Add(new InvalidCredentialsValidator());
            ScrapeSessionDataValidator scrapeSessionDataValidator = new ScrapeSessionDataValidator(validatorList);
            //Act
            scrapeSessionDataValidator.ValidateScrapeData(interpretedScrapeSessionDataPair,Guid.NewGuid(),Guid.NewGuid());
            //Assert
            //There is an expected exception thrown

        }
    }
}
