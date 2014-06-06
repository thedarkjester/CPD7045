using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class ScrapingErrorRetryConfigurationTests
    {
        private int numberOfRetries = 1;
        private int responseCode = 0;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_LessThanZero_For_ResponseCode_When_Constructing_A_ScrapingErrorRetryConfiguration_ExceptionIsThrown()
        {
            //arrange
            responseCode = -1;

            //act
            ScrapingErrorRetryConfiguration configuration = new ScrapingErrorRetryConfiguration(responseCode, numberOfRetries);

            //assert
            //Exception Expected
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_LessThanZero_For_NumberOfRetries_When_Constructing_A_ScrapingErrorRetryConfiguration_ExceptionIsThrown()
        {
            //arrange
            numberOfRetries = -1;

            //act
            ScrapingErrorRetryConfiguration configuration = new ScrapingErrorRetryConfiguration(responseCode, numberOfRetries);

            //assert
            //Exception Expected
        }

        [TestMethod]
        public void Given_AValidValue_For_NumberOfRetries_When_Constructing_A_ScrapingErrorRetryConfiguration_CreationIsSuccessful()
        {
            //arrange
            numberOfRetries = 1;
            responseCode = 0;

            //act
            ScrapingErrorRetryConfiguration configuration = new ScrapingErrorRetryConfiguration(responseCode, numberOfRetries);

            //assert
            Assert.IsTrue(configuration.RetryInterval == 1);
            Assert.IsTrue(configuration.ResponseCode == 0);
            ;
        }
    }
}