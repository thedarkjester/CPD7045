using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests.BillingCompanyTests
{
    [TestClass]
    public class ScrapingLoadManagementConfigurationTests
    {
        private int concurrentScrapes = 10;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Zero_When_Constructing_A_ScrapingLoadManagementConfiguration_ExceptionIsThrown()
        {
            //arrange
            concurrentScrapes = 0;

            //act
            ScrapingLoadManagementConfiguration configuration = new ScrapingLoadManagementConfiguration(concurrentScrapes);

            //assert
            //Exception Expected
        }

        [TestMethod]
        public void Given_AValidValue_When_Constructing_A_ScrapingLoadManagementConfiguration_ObjectIsCreatedCorrectly()
        {
            //arrange
            concurrentScrapes = 1;

            //act
            ScrapingLoadManagementConfiguration configuration = new ScrapingLoadManagementConfiguration(concurrentScrapes);

            //assert
            Assert.IsTrue(configuration.ConcurrentScrapes == 1);
        }
    }
}