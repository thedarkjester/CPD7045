using System;
using Aps.IntegrationEvents.Queries.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.IntegrationTests.QueryTests.Events
{
    [TestClass]
    public class LatestEventsByNamespaceQueryConstructionTests
    {
        private string nameSpace = typeof(GetLatestEventsBySubScribedEventTypeQuery).FullName;
        private int rowIdentifier = 1;

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void When_Constructing_An_EventsByNameSpaceQuery_TypeIsNotEmpty()
        {
            //arrange
            nameSpace = string.Empty;

            //act
            GetLatestEventsBySubScribedEventTypeQuery query = new GetLatestEventsBySubScribedEventTypeQuery(nameSpace, rowIdentifier);

            //assert
            //see exception
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void When_Constructing_An_EventsByNameSpaceQuery_RowIdentifierIsGreaterThanZero()
        {
            //arrange
            rowIdentifier = 0;

            //act
            GetLatestEventsBySubScribedEventTypeQuery query = new GetLatestEventsBySubScribedEventTypeQuery(nameSpace, rowIdentifier);

            //assert
            //see exception
        }
    }
}
