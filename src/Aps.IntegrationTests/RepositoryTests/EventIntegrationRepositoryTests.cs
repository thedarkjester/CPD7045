using System.Collections.Generic;
using System.Linq;
using Aps.Fakes;
using Aps.Integration;
using Aps.Integration.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.IntegrationTests.RepositoryTests
{
    [TestClass]
    public class EventIntegrationRepositoryTests
    {
        private IEventIntegrationRepository repository = new EventIntegrationRepositoryFake();

        [TestMethod]
        public void GivenARepository_Adding_To_The_EventRepository_IncreasesTheEventCount()
        {
            // arrange
            repository = new EventIntegrationRepositoryFake(); // making sure it is empty
            var newEvent = new IntegrationEvent("test", new byte[1]);

            // act
            repository.StoreEvent(newEvent);

            // assert
            Assert.IsTrue(repository.GetAllEvents().Count() == 1);
        }

        [TestMethod]
        public void GivenARepository_Adding_To_The_EventRepository_IncreasesTheEventsRowVersion()
        {
            // arrange
            repository = new EventIntegrationRepositoryFake(); // making sure it is empty
            var newEvent = new IntegrationEvent("test", new byte[1]);

            // act
            repository.StoreEvent(newEvent);

            // assert
            Assert.IsTrue(newEvent.RowVersion == 1);
        }

        [TestMethod]
        public void GivenARepository_Adding_To_The_EventRepository_IncreasesTheEventsRowVersions()
        {
            // arrange
            repository = new EventIntegrationRepositoryFake(); // making sure it is empty
            var newEvent = new IntegrationEvent("test", new byte[1]);
            var newEvent2 = new IntegrationEvent("test2", new byte[1]);
            var newEvent3 = new IntegrationEvent("test3", new byte[1]);

            // act
            repository.StoreEvent(newEvent);
            repository.StoreEvent(newEvent2);
            repository.StoreEvent(newEvent3);

            // assert
            Assert.IsTrue(newEvent.RowVersion == 1);
            Assert.IsTrue(newEvent2.RowVersion == 2);
            Assert.IsTrue(newEvent3.RowVersion == 3);
        }

        [TestMethod]
        public void GivenANamespaceAndALastProcessedNumber_TheRepositoryReturnsTheCorrectEntries()
        {
            // arrange
            repository = new EventIntegrationRepositoryFake(); // making sure it is empty
            var newEvent = new IntegrationEvent("test", new byte[1]);
            var newEvent2 = new IntegrationEvent("test", new byte[1]);
            var newEvent3 = new IntegrationEvent("test", new byte[1]);
            repository.StoreEvent(newEvent);
            repository.StoreEvent(newEvent2);
            repository.StoreEvent(newEvent3);

            // act
            IEnumerable<IntegrationEvent> events = repository.GetLatestEvents(1, "test");

            // assert
            Assert.IsTrue(events.Count() == 2);
        }

        [TestMethod]
        public void GivenANamespaceAndALastProcessedNumber_TheRepositoryReturnsNoEntries_WhenNone()
        {
            // arrange
            repository = new EventIntegrationRepositoryFake(); // making sure it is empty
            var newEvent = new IntegrationEvent("test", new byte[1]);
            var newEvent2 = new IntegrationEvent("test", new byte[1]);
            var newEvent3 = new IntegrationEvent("test", new byte[1]);
            repository.StoreEvent(newEvent);
            repository.StoreEvent(newEvent2);
            repository.StoreEvent(newEvent3);

            // act
            IEnumerable<IntegrationEvent> events = repository.GetLatestEvents(3, "test");

            // assert
            Assert.IsTrue(!events.Any());
        }
    }
}
