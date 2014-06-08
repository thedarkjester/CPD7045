using System;
using Aps.Customers.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Aps.Shared.Tests.CustomerTests
{
    [TestClass]
    public class CustomerTelephoneTests
    {
        private string tel = "test";

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void WhenConstructingGivenAnEmptyStringAnArgumentExceptionIsThrown()
        {
            //arrange
            tel = "";

            //act

            CustomerTelephone customerTel = new CustomerTelephone(tel);

            //assert
            //Exception Expected
        }
    }
}