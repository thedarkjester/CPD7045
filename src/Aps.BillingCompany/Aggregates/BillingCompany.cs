using System.Collections.Generic;
using Aps.BillingCompanies.ValueObjects;
using Aps.DomainBase;

namespace Aps.BillingCompanies.Aggregates
{
    public class BillingCompany : Aggregate
    {
        private readonly List<OpenClosedWindow> openClosedWindows;

        public IEnumerable<OpenClosedWindow> OpenClosedWindows { get { return openClosedWindows; } }

        public BillingCompany()
        {
            this.openClosedWindows = new List<OpenClosedWindow>();
        }

    }
}
