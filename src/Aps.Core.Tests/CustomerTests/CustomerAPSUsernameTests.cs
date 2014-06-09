using System;
using Aps.Customers.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Shared.Tests.CustomerTests
{
    [TestClass]
    public class CustomerAPSUserNameTests
    {
        private string username = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            username = "";

            //act

            CustomerAPSUsername customerUsername = new CustomerAPSUsername(username);

            //assert
            //Exception Expected
        }
    }
}