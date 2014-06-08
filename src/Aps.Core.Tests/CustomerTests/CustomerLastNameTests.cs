using System;
using Aps.Customers.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Shared.Tests.CustomerTests
{
    [TestClass]
    public class CustomerLastNameTests
    {
        private string lastName = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            lastName = "";

            //act

            CustomerLastName customerLastName = new CustomerLastName(lastName);

            //assert
            //Exception Expected
        }
    }
}