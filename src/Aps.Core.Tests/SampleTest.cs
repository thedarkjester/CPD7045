using System;
using Aps.Scheduling.ApplicationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Domain.Tests
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        
        // how about this is a naming convention
        // Action_StateOrObject_Expection
        [ExpectedException(typeof(ArgumentException))]
        public void PassingEmptyGuidId_ToTestConstructor_ThrowsInvalidArgumentException()
        {
            // Arrange
            // nothing to arrange as it is a constructor test

            // Act
             var newTestConstructor = new TestConstructor(Guid.Empty);

            // Assert
            // Assertion is in the attributes
        }
    }
}
