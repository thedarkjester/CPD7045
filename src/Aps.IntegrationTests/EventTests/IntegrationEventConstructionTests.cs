using System;
using Aps.IntegrationEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.IntegrationTests.EventTests
{
    [TestClass]
    public class IntegrationEventConstructionTests
    {

        //arrange
        private DateTime startDate;
        private DateTime endDate;
        private bool isOpen;
        private int concurrentScrapingLimit;
        private Guid billingCompanyId;

        [TestInitialize]
        public void Setup()
        {
            //arrange
            startDate = DateTime.Now;
            endDate = DateTime.Now;
            isOpen = false;
            concurrentScrapingLimit = 1;
            billingCompanyId = Guid.NewGuid();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructionOfBillingCompanyAddedOpenClosedWindowEvent_EmptyGuidParameterIsInvalid_ExceptionIsThrown()
        {
            //arrange
            billingCompanyId = Guid.Empty;

            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void WhenConstructionOfBillingCompanyAddedOpenClosedWindowEvent_IsOpenAndScrapingLimitCombinationIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            isOpen = true;
            concurrentScrapingLimit = 0;

            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructionOfBillingCompanyAddedOpenClosedWindowEvent_StartDatePastTenseIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            startDate = startDate.AddDays(-2);

            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructionOfBillingCompanyAddedOpenClosedWindowEvent_EndDatePastTenseIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddDays(-2);

            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void WhenConstructionOfBillingCompanyAddedOpenClosedWindowEvent_StartDateIsLessThanEndDate_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddDays(2);
            startDate = startDate.AddDays(3);

            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void WhenConstructionOfBillingCompanyAddedOpenClosedWindowEvent_StartDateIsLessThanEndDateInLowestGranularity_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddMilliseconds(2);
            startDate = startDate.AddMilliseconds(3);

            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);

            //assert
            // see expected Exception
        }
    }
}
