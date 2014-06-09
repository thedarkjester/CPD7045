using Aps.AccountStatements;
using Aps.AccountStatements.Entities;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Scheduling.ApplicationService.Entities;
using Aps.Scheduling.ApplicationService.InternalEvents;
using Aps.Scheduling.ApplicationService.Interpreters;
using Aps.Scheduling.ApplicationService.Services;
using Aps.Scheduling.ApplicationService.Validation;
using Aps.Scraping;
using Aps.Scraping.Scrapers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.ScrapeOrchestrators
{
    public class StatementScrapeOrchestrator : ScrapeOrchestrator
    {
        readonly IEventAggregator eventAggregator;
        readonly AccountStatementComposer accountStatementComposer;
        readonly FailureHandler failureHandler;
        readonly EventIntegrationService eventIntegrationService;
        readonly IScrapeLoggingRepository scrapeLoggingRepository;
        readonly IWebScraper webScraper;
        readonly ScrapeSessionDataValidator scrapeSessionDataValidator;
        readonly IAccountStatementRepository accountStatementRepository;
        public StatementScrapeOrchestrator(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, AccountStatementComposer accountStatementComposer, FailureHandler failureHandler, IScrapeLoggingRepository scrapeLoggingRepository, IWebScraper webScraper, ScrapeSessionDataValidator scrapeSessionDataValidator, IAccountStatementRepository accountStatementRepository)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.accountStatementComposer = accountStatementComposer;
            this.failureHandler = failureHandler;
            this.scrapeLoggingRepository = scrapeLoggingRepository;
            this.webScraper = webScraper;
            this.scrapeSessionDataValidator = scrapeSessionDataValidator;
            this.accountStatementRepository = accountStatementRepository;
        }

        public override void Orchestrate(ScrapeOrchestratorEntity scrapeOrchestratorEntity)
        {
            Guid scrapeSessionId = Guid.NewGuid();
            Guid customerId = scrapeOrchestratorEntity.CustomerId;
            Guid billingCompanyId = scrapeOrchestratorEntity.BillingCompanyId;            
            Guid queueId = scrapeOrchestratorEntity.QueueId;
            bool hasFailed = false;
            var scrapeSessionData = string.Empty;
            try
            {
                eventIntegrationService.Publish(new ScrapeSessionStarted(scrapeSessionId, customerId, billingCompanyId));
                scrapeSessionData = webScraper.Scrape(scrapeOrchestratorEntity.Url, scrapeOrchestratorEntity.Username, scrapeOrchestratorEntity.Password, scrapeOrchestratorEntity.Pin);
                eventIntegrationService.Publish(new ScrapeSessionDataRetrievalCompleted(scrapeSessionId, customerId, billingCompanyId));

                var transformedResults = new ScrapeSessionXMLToDataPairConverter().ConvertXmlToScrapeSessionDataPairs(scrapeSessionData);
                eventIntegrationService.Publish(new ScrapeSessionDataInterpretered(scrapeSessionId, customerId, billingCompanyId));

                scrapeSessionDataValidator.ValidateScrapeData(transformedResults, customerId, billingCompanyId);
                eventIntegrationService.Publish(new ScrapeSessionDataValidated(scrapeSessionId, customerId, billingCompanyId));

                AccountStatement accountStatement = accountStatementComposer.BuildAccountStatement(customerId, billingCompanyId, transformedResults.Select(x => x.keyValuePair).ToList());
                accountStatementRepository.StoreAccountStatement(accountStatement);
                eventIntegrationService.Publish(new ScrapeSessionStatementComposed(scrapeSessionId, customerId, billingCompanyId, accountStatement.Id, accountStatement.StatementDate.DateOfStatement));

                eventAggregator.Publish(new ScrapeSessionSuccessful(queueId, accountStatement.StatementDate.DateOfStatement));
                eventIntegrationService.Publish(new ScrapeSessionCompletedSuccessfully(scrapeSessionId, customerId, billingCompanyId));
            }
            catch (DataScraperException dse)
            {
                failureHandler.ProcessNewFailure(customerId, billingCompanyId, dse.Error);
                eventIntegrationService.Publish(new ScrapeSessionCompletedWithErrors(scrapeSessionId, customerId, billingCompanyId, dse.Error.ToString()));
                eventAggregator.Publish(new ScrapeSessionFailed(queueId, dse.Error));
                hasFailed = true;
            }
            catch (DuplicateStatementException)
            {
                eventAggregator.Publish(new ScrapeSessionDuplicateStatement(queueId));
                eventIntegrationService.Publish(new ScrapeSessionDuplicateStatementReceived(scrapeSessionId, customerId, billingCompanyId));
                hasFailed = true;
            }
            catch (Exception e)
            {
                failureHandler.ProcessNewFailure(customerId, billingCompanyId, Integration.EnumTypes.ScrapingErrorResponseCodes.Unknown);
                eventIntegrationService.Publish(new ScrapeSessionCompletedWithErrors(scrapeSessionId, customerId, billingCompanyId, e.Message));
                eventAggregator.Publish(new ScrapeSessionFailed(queueId, Integration.EnumTypes.ScrapingErrorResponseCodes.Unknown));
                hasFailed = true;
            }
            finally
            {
                scrapeLoggingRepository.StoreScrape(new ScrapingLog(scrapeSessionId, customerId, billingCompanyId, hasFailed, scrapeSessionData));
            }
        }
    }
}
