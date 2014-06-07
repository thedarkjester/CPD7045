using Aps.Core.InternalEvents;
using Aps.Core.Services;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Shared.Tests.ValidatorTests;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.ScrapeOrchestrators
{
    public class StatementScrapeOrchestrator : ScrapeOrchestrator
    {
        private readonly IEventAggregator eventAggregator;
        private readonly AccountStatementComposer accountStatementComposer;
        private readonly FailureHandler failureHandler;
        private readonly EventIntegrationService eventIntegrationService;
        public StatementScrapeOrchestrator(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService,  AccountStatementComposer accountStatementComposer, FailureHandler failureHandler)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.accountStatementComposer = accountStatementComposer;
            this.failureHandler = failureHandler;
        }

        public override void Orchestrate()
        {
            Guid customerId = Guid.NewGuid();
            Guid billingCompanyId = Guid.NewGuid();
            Guid scrapeSessionId = Guid.NewGuid();
            try
            {
                //Initiate scrape session
                var scrapeSessionData = string.Empty; //create interface
                eventIntegrationService.Publish(new ScrapeSessionDataRetrievalCompleted(customerId, billingCompanyId));
                
                var transformedResults = new Interpreter().TransformResults(scrapeSessionData);
                //fire event
                //check if duplicate statement
                //if there is duplicate
                //fire event
                //call failure handler 
                //return

                eventAggregator.Publish(new ScrapeSessionDuplicateStatement(scrapeSessionId));
                var validatedResults = new ScrapeSessionDataValidator().validateScrapeData(transformedResults);

                //fire event
                accountStatementComposer.BuildAccountStatement(customerId, billingCompanyId, new List<KeyValuePair<string, object>>());
                //fire event
                eventAggregator.Publish(new ScrapeSessionSuccessful(scrapeSessionId, customerId, billingCompanyId));
                eventIntegrationService.Publish(new CustomerScrapeSessionSuccessful(customerId, billingCompanyId));
            }
            catch (ScrapperException se)
            {

                failureHandler.ProcessNewFailure(customerId, billingCompanyId, se.Error);
                eventIntegrationService.Publish(new CustomerScrapeSessionFailed(customerId, billingCompanyId, se.Error));
                eventAggregator.Publish(new ScrapeSessionFailed(scrapeSessionId, se.Error.ToString()));
                
            }
            finally
            {
                //log data
            }
        }
    }
}
