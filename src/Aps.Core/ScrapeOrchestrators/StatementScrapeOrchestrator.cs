using Aps.Core.InternalEvents;
using Aps.Core.Services;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Scraping;
using Aps.Scraping.Scrapers;
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
        readonly IEventAggregator eventAggregator;
        readonly AccountStatementComposer accountStatementComposer;
        readonly FailureHandler failureHandler;
        readonly EventIntegrationService eventIntegrationService;
        readonly IScrapeLoggingRepository scrapeLoggingRepository;
        readonly IWebScraper webScraper;
        public StatementScrapeOrchestrator(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, AccountStatementComposer accountStatementComposer, FailureHandler failureHandler, IScrapeLoggingRepository scrapeLoggingRepository, IWebScraper webScraper)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.accountStatementComposer = accountStatementComposer;
            this.failureHandler = failureHandler;
            this.scrapeLoggingRepository = scrapeLoggingRepository;
            this.webScraper = webScraper;
        }

        public override void Orchestrate()
        {
            Guid customerId = Guid.NewGuid();
            Guid billingCompanyId = Guid.NewGuid();
            Guid scrapeSessionId = Guid.NewGuid();
            Guid queueId = Guid.NewGuid();
            bool isDuplicateStatment = false;
            bool hasFailed = false;
            var scrapeSessionData = string.Empty;
            try
            {
                eventIntegrationService.Publish(new ScrapeSessionStarted(scrapeSessionId, customerId, billingCompanyId));
                scrapeSessionData = webScraper.Scrape(null, null, null);
                eventIntegrationService.Publish(new ScrapeSessionDataRetrievalCompleted(scrapeSessionId, customerId, billingCompanyId));

                var transformedResults = new ScrapeSessionXMLToDataPairConverter().ConvertXmlToScrapeSessionDataPairs(scrapeSessionData);
                eventIntegrationService.Publish(new ScrapeSessionDataInterpretered(scrapeSessionId, customerId, billingCompanyId));

                //check if duplicate statement
                if (isDuplicateStatment)
                {
                    eventAggregator.Publish(new ScrapeSessionDuplicateStatement(queueId));
                    eventIntegrationService.Publish(new ScrapeSessionDuplicateStatementReceived(scrapeSessionId, customerId, billingCompanyId));
                    //failureHandler.ProcessNewFailure(customerId, billingCompanyId, Integration.EnumTypes.ScrapingErrorResponseCodes.Unknown);
                    return;
                }

                //var validatedResults = new ScrapeSessionDataValidator().validateScrapeData(transformedResults);
                eventIntegrationService.Publish(new ScrapeSessionDataValidated(scrapeSessionId, customerId, billingCompanyId));

                accountStatementComposer.BuildAccountStatement(customerId, billingCompanyId, new List<KeyValuePair<string, object>>());
                eventIntegrationService.Publish(new ScrapeSessionStatementComposed(scrapeSessionId, customerId, billingCompanyId));

                eventAggregator.Publish(new ScrapeSessionSuccessful(queueId));
                eventIntegrationService.Publish(new ScrapeSessionCompletedSuccessfully(scrapeSessionId, customerId, billingCompanyId));
            }
            catch (DataScraperException dse)
            {
                failureHandler.ProcessNewFailure(customerId, billingCompanyId, dse.Error);
                eventIntegrationService.Publish(new ScrapeSessionCompletedWithErrors(scrapeSessionId, customerId, billingCompanyId, dse.Error.ToString()));
                eventAggregator.Publish(new ScrapeSessionFailed(queueId, dse.Error.ToString()));
                hasFailed = true;
            }
            catch (Exception e)
            {
                failureHandler.ProcessNewFailure(customerId, billingCompanyId, Integration.EnumTypes.ScrapingErrorResponseCodes.Unknown);
                eventIntegrationService.Publish(new ScrapeSessionCompletedWithErrors(scrapeSessionId, customerId, billingCompanyId, e.Message));
                eventAggregator.Publish(new ScrapeSessionFailed(queueId, Integration.EnumTypes.ScrapingErrorResponseCodes.Unknown.ToString()));
                hasFailed = true;
            }
            finally
            {
                scrapeLoggingRepository.StoreScrape(new ScrapingLog(scrapeSessionId, customerId, billingCompanyId, hasFailed, scrapeSessionData));
            }
        }
    }
}
