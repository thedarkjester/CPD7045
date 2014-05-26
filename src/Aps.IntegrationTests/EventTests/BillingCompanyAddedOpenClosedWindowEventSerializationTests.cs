using System;
using System.Linq;
using System.Reflection;
using Aps.IntegrationEvents;
using Aps.IntegrationEvents.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.IntegrationTests.EventTests
{
    [TestClass]
    public class BillingCompanyAddedOpenClosedWindowEventSerializationTests
    {
        // default valid variables
        private DateTime startDate;
        private DateTime endDate;
        private bool isOpen;
        private int concurrentScrapingLimit;
        private Guid billingCompanyId;

        private BinaryEventSerializer eventSerializer;
        private BinaryEventDeSerializer eventDeSerializer;

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        [TestInitialize]
        public void Setup()
        {
            //arrange
            startDate = DateTime.Now.AddMinutes(2);
            endDate = DateTime.Now.AddMinutes(3);
            isOpen = false;
            concurrentScrapingLimit = 1;
            billingCompanyId = Guid.NewGuid();

            eventSerializer = new BinaryEventSerializer();
            eventDeSerializer = new BinaryEventDeSerializer();
        }

        [TestMethod]
        public void Given_A_BillingCompanyAddedOpenClosedWindowEvent_It_AllValuesAreSerializable()
        {
            //arrange
            
            //act
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);
            
            //assert

            // all properties can be written and read
            PropertyInfo[] properties = billingCompanyAddedOpenClosedWindow.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in properties)
            {
                Assert.IsTrue(propertyInfo.CanRead);
                Assert.IsTrue(propertyInfo.CanWrite);
            }
        }

        [TestMethod]
        public void Given_A_SerializedBillingCompanyAddedOpenClosedWindowEvent_It_AllValuesAreDeSerializedCorrectly()
        {
            //arrange
            var billingCompanyAddedOpenClosedWindow = new BillingCompanyAddedOpenClosedWindow(startDate, endDate, isOpen, concurrentScrapingLimit, billingCompanyId);
            var serializedEvent = eventSerializer.SerializeMessage(billingCompanyAddedOpenClosedWindow);

            //act
            var deserializedMessage = eventDeSerializer.DeSerializeMessage(serializedEvent);

            //assert
            PropertyInfo[] properties = billingCompanyAddedOpenClosedWindow.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in properties)
            {
                var inValue = GetPropValue(billingCompanyAddedOpenClosedWindow, propertyInfo.Name);
                var outValue = GetPropValue(deserializedMessage, propertyInfo.Name);

                Assert.IsTrue(inValue.Equals(outValue),propertyInfo.Name + " does not match");
            }
        }

    }
}