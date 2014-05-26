using Seterlund.CodeGuard;

namespace Aps.IntegrationEvents.Queries.Events
{
    public class GetLatestEventsBySubScribedEventTypeQuery
    {
        private readonly string nameSpace;
        private readonly int rowIdentifier;

        public GetLatestEventsBySubScribedEventTypeQuery(string nameSpace, int rowIdentifier)
        {
            Guard.That(nameSpace).IsNotEmpty();
            Guard.That(rowIdentifier).IsGreaterThan(0);

            this.nameSpace = nameSpace;
            this.rowIdentifier = rowIdentifier;
        }
    }
}