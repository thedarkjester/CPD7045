using System;
using Aps.BillingCompanies.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyLifeCycleTests
    {
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void WhenConstructingABillingCompanyLifeCycle_GivenAZeroForDaysPerCycle_ThrowsException()
        {
            //arrange
            int daysPerBillingCycle = 0;
            //act
            BillingLifeCycle billingLifeCycle = new BillingLifeCycle(daysPerBillingCycle, 0, 0);

            //assert

        }
    }
}
