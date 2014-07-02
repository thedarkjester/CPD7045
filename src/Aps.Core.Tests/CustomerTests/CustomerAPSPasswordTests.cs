using System;
using Aps.Customers.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Domain.Tests.CustomerTests
{
    [TestClass]
    public class CustomerAPSPasswordTests
    {
        private string password = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            password = "";

            //act

            CustomerAPSPassword customerPassword = new CustomerAPSPassword(password);

            //assert
            //Exception Expected
        }
    }
}