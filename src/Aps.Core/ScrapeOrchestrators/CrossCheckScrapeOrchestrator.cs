using Aps.Integration;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.ScrapeOrchestrators
{
    public class CrossCheckScrapeOrchestrator : ScrapeOrchestrator
    {
         private readonly IEventAggregator eventAggregator;
        private readonly FailureHandler failureHandler;
        private readonly EventIntegrationService eventIntegrationService;
        public CrossCheckScrapeOrchestrator(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, FailureHandler failureHandler)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.failureHandler = failureHandler;
        }

        public override void Orchestrate()
        {
            //intiate cross check
            //check result
            //fire successful/failure event based on result
            //called failure handler when result is false
        }
    }
}
