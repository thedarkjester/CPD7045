using System;
using Aps.Customers.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Domain.Tests.CustomerTests
{
    [TestClass]
    public class CustomerFirstNameTests
    {
        private string firstName = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            firstName = "";

            //act

            CustomerFirstName customerFirstname = new CustomerFirstName(firstName);

            //assert
            //Exception Expected
        }
    }
}