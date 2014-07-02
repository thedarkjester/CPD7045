using System;
using Aps.Customers.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Domain.Tests.CustomerTests
{
    [TestClass]
    public class CustomerEmailAddressTests
    {
        private string email = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            email = "";

            //act

            CustomerEmailAddress customerEmail = new CustomerEmailAddress(email);

            //assert
            //Exception Expected
        }
    }
}