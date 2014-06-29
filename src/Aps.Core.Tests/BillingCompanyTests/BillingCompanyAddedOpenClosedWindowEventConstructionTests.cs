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
            var openClosedWindow = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }

        [TestMethod]
        public void Given_Two_OpenClosedWindowsInstancesWithEqualData_When_Compared_Then_They_Are_Equal()
        {
            isOpen = true;
            concurrentScrapingLimit = 10;
            startDate = startDate.AddDays(2);
            endDate = startDate.AddDays(3);

            //act
            var openClosedWindow1 = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);
            var openClosedWindow2 = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            Assert.IsTrue(openClosedWindow1 == openClosedWindow2);
            Assert.IsTrue(openClosedWindow1.Equals(openClosedWindow2));
        }

        [TestMethod]
        public void Given_Two_OpenClosedWindowsInstancesWithDifferentData_When_Compared_Then_They_Are_NotEqual()
        {
            isOpen = true;
            concurrentScrapingLimit = 10;
            startDate = startDate.AddDays(2);
            endDate = startDate.AddDays(3);

            //act
            var openClosedWindow1 = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            endDate = startDate.AddDays(4);

            var openClosedWindow2 = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            Assert.IsTrue(openClosedWindow1 != openClosedWindow2);
            Assert.IsTrue(!openClosedWindow1.Equals(openClosedWindow2));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_StartDatePastTenseIsInvalid_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            startDate = startDate.AddDays(-2);

            //act
            var openClosedWindow = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

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
            var openClosedWindow = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

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
            var openClosedWindow = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_When_The_StartDateIsLessThanEndDateInLowestGranularity_ExceptionIsThrown()
        {
            //arrange ( you should be able to scrape if the window is open )
            endDate = endDate.AddMilliseconds(200);
            startDate = startDate.AddMilliseconds(3000);

            //act
            var openClosedWindow = new OpenClosedScrapingWindow(startDate, endDate, isOpen, concurrentScrapingLimit);

            //assert
            // see expected Exception
        }
    }
}
