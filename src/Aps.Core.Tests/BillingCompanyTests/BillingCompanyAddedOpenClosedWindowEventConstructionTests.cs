using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyClosedWindowConstructionTests
    {
        // default valid variables
        private DateTime startDate;
        private DateTime endDate;
        private bool isOpen;
        private int concurrentScrapingLimit;

        [TestInitialize]
        public void Setup()
        {
            //arrange
            startDate = DateTime.Now;
            endDate = DateTime.Now;
            isOpen = false;
            concurrentScrapingLimit = 1;
        }


        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_IsOpenAndScrapingLimitCombinationIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            isOpen = true;
            concurrentScrapingLimit = 0;

            //act
            var openClosedWindow = new OpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_StartDatePastTenseIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            startDate = startDate.AddDays(-2);

            //act
            var openClosedWindow = new OpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_EndDatePastTenseIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddDays(-2);

            //act
            var openClosedWindow = new OpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_StartDateIsLessThanEndDate_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddDays(2);
            startDate = startDate.AddDays(3);

            //act
            var openClosedWindow = new OpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_StartDateIsLessThanEndDateInLowestGranularity_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddMilliseconds(2);
            startDate = startDate.AddMilliseconds(3);

            //act
            var openClosedWindow = new OpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }
    }
}
