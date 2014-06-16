using Aps.Integration;
using Aps.Integration.Events;
using Aps.Scheduling.ApplicationService.InternalEvents;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Fakes
{
    public class ExternalSchedularEventsFake
    {
        private EventIntegrationService eventIntegrationService;
        public ExternalSchedularEventsFake(EventIntegrationService eventIntegrationService)
        {
            this.eventIntegrationService = eventIntegrationService;
        }

        public void getCustomerBillingAccountAddedEvent(Guid customerId, Guid billingCompanyId)
        {
            eventIntegrationService.Publish(new CustomerBillingAccountAdded(customerId, billingCompanyId));
        }

        public void getBillingAccountDeletedFromCustomerEvent(Guid customerId, Guid billingCompanyId)
        {
            eventIntegrationService.Publish(new BillingAccountDeletedFromCustomer(customerId, billingCompanyId));
        }

        public void getScrapeSessionSuccessfullEvent(Guid billingCompanyId)
        {
            eventIntegrationService.Publish(new ScrapingScriptUpdated(billingCompanyId));
        }

    }
}
