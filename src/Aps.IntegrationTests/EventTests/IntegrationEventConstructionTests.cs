using System;
using Aps.IntegrationEvents.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.IntegrationTests.EventTests
{
    [TestClass]
    public class IntegrationEventConstructionTests
    {
        private string nameSpaceName = "Default";
        private byte[] serializedEvent =new byte[1];

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Given_NullByteArrayWhenConstructing_ExceptionIsThrown()
        {
            //arrange
            serializedEvent = null;

            IntegrationEvent @event = new IntegrationEvent(nameSpaceName, serializedEvent);

            serializedEvent = null;
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_EmptyByteArrayWhenConstructing_ExceptionIsThrown()
        {
            //arrange
            serializedEvent = new byte[0];

            IntegrationEvent @event = new IntegrationEvent(nameSpaceName, serializedEvent);

            serializedEvent = null;
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_EmptyNameSpace_When_Constructing_ExceptionIsThrown()
        {
            //arrange
            nameSpaceName = "";

            IntegrationEvent @event = new IntegrationEvent(nameSpaceName, serializedEvent);

            serializedEvent = null;
        }
    }
}
