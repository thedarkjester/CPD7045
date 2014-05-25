using System.Collections.Generic;
using Aps.BillingCompanies.ValueObjects;
using Aps.DomainBase;
using Caliburn.Micro;

namespace Aps.BillingCompanies.Aggregates
{
    public class BillingCompany : Aggregate
    {
        private readonly IEventAggregator eventAggregator;
        private readonly List<OpenClosedWindow> openClosedWindows;

        public IEnumerable<OpenClosedWindow> OpenClosedWindows { get { return openClosedWindows; } }

        public IEventAggregator EventAggregator { get; set; }
       
        public BillingCompany(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.openClosedWindows = new List<OpenClosedWindow>();
        }
    }
}
