using Aps.Integration;
using Caliburn.Micro;

namespace Aps.Core.Services
{
    public class AccountStatementService
    {
        private readonly IEventAggregator eventAggregator;
        private readonly EventIntegrationService eventIntegrationService;

        public AccountStatementService(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
        }
    }
}