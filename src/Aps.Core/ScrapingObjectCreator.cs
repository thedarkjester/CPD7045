using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core
{
    public class ScrapingObjectCreator
    {
        private readonly IEventAggregator eventAggregator;

        public ScrapingObjectCreator(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public ScrapingObject GetNewScrapingObject(Guid customerId, Guid billingCompanyId, string URL, string plainText, string hiddenText)
        {
            return new ScrapingObject(customerId, billingCompanyId, URL, plainText, hiddenText);
        }
    }
}
