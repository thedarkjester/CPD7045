using Aps.BillingCompanies;
using Aps.Core.InternalEvents;
using Aps.Customers;
using Aps.Integration;
using Aps.Integration.Events;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;
using System.Threading;
using System.Threading.Tasks;
using System;
using Aps.Fakes;
using Aps.Scraping;

namespace Aps.Core
{
    public class SchedulingEngine : IHandle<ScrapeSessionFailed>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly EventIntegrationService messageSendAndReceiver;
        private BillingCompanyOpenClosedWindowsQuery billingCompanyOpenClosedWindowsQuery;
        private IScrapingObjectRepository scrapingObjectRepositoryFake;
        private BillingCompanyScrapingLoadManagementConfigurationQuery billingCompanyScrapingLoadManagementConfigurationQuery;
        public Dictionary<Guid, int> currentNumberOfThreadsPerBillingCompany;
        //private Guid ScrapeQueueId;
        public int maxAllowedServerScrapes;
        public List<ScrapingObject> scrapeElementsQueue;
        public List<ScrapingObject> scrapeElementsRunning;
        public CancellationToken cancellationToken;
        readonly ScrapeSessionInitiator scrapeSessionInitiator;

        public SchedulingEngine(IEventAggregator eventAggregator, EventIntegrationService messageSendAndReceiver, IScrapingObjectRepository scrapingObjectRepositoryFake, BillingCompanyOpenClosedWindowsQuery billingCompanyOpenClosedWindowsQuery, BillingCompanyScrapingLoadManagementConfigurationQuery billingCompanyScrapingLoadManagementConfigurationQuery, ScrapeSessionInitiator scrapeSessionInitiator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.scrapeElementsQueue = new List<ScrapingObject>();
            this.billingCompanyOpenClosedWindowsQuery = billingCompanyOpenClosedWindowsQuery;
            this.scrapingObjectRepositoryFake = scrapingObjectRepositoryFake;
            this.billingCompanyScrapingLoadManagementConfigurationQuery = billingCompanyScrapingLoadManagementConfigurationQuery;
            this.currentNumberOfThreadsPerBillingCompany = new Dictionary<Guid, int>();
            this.messageSendAndReceiver = messageSendAndReceiver;
            maxAllowedServerScrapes = 20;
            this.scrapeElementsQueue = new List<ScrapingObject>();
            this.scrapeElementsRunning = new List<ScrapingObject>();
            this.scrapeSessionInitiator = scrapeSessionInitiator;
        }


        public void Start()
        {
            //messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(NewCustomerBillingCompanyAccount).FullName);
           // messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(CustomerBillingAccountAdded).FullName);
            Scrape();
        }
        
        private void Scrape()
        {
            OpenClosedWindowDto openClosedWindowDto;
            int maxAllowedThreadsForSpecificBillingCompany = 0;

            while (true)
            {
                scrapeElementsQueue = getNewScrapeQueueWithoutCompletedItems();
                if (scrapeElementsQueue == null)
                {
                    continue;
                }

                foreach (var scrapingQueueElement in scrapeElementsQueue)
                {
                    if (getNumberOfThreadsAvailableOnServer() == 0)
                        break;

                    openClosedWindowDto = GetCurrentOpenClosedWindowDtoByBillingCompany(scrapingQueueElement.billingCompanyId);
                    maxAllowedThreadsForSpecificBillingCompany = getMaxAllowedThreadsForSpecificBillingCompany(openClosedWindowDto, scrapingQueueElement.billingCompanyId);

                    if (maxAllowedThreadsForSpecificBillingCompany == 0)
                        RescheduleItem(scrapingQueueElement, openClosedWindowDto.EndDate);

                    else if (maxAllowedThreadsForSpecificBillingCompany - NumberOfThreadsCurrentlyUsedByBillingCompanyId(scrapingQueueElement.billingCompanyId) > 0)
                    {
                        currentNumberOfThreadsPerBillingCompany[scrapingQueueElement.billingCompanyId] += 1;
                        scrapeElementsRunning.Add(scrapingQueueElement);
                        // This is where I pass the work to Jignesh! Once it exists, add it. 
                        //scrapeSessionInitiator.InitiateNewScrapeSession(scrapingQueueElement);

                    }
                }
                Thread.Sleep(6000); // Entire QueueProcessed or threads full, sleep a while and then start with a new Queue
            }
        }

        public void addNewCurrentNumberOfThreadsPerBillingCompany(Guid billingCompanyId)
        {
            currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 0);
        }


        public List<ScrapingObject> getNewScrapeQueueWithoutCompletedItems()
        {
            List<ScrapingObject> sortedScrapingQueueItemsWithoutCompletedScrapes = scrapingObjectRepositoryFake.GetAllScrapingObjects().Where(item => item.ScheduledDate <= DateTime.UtcNow).ToList();
            List<ScrapingObject> completedScrapingQueue = scrapingObjectRepositoryFake.GetCompletedScrapeQueue().ToList();

            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
            {
                return new List<ScrapingObject>();
            }

            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => !completedScrapingQueue.Any(y => y.queueId == x.queueId));
            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => !scrapeElementsRunning.Any(y => y.queueId == x.queueId));
            scrapingObjectRepositoryFake.ClearCompletedScrapeList();
           // sortedScrapingQueueItemsWithoutCompletedScrapes = sortedScrapingQueueItemsWithoutCompletedScrapes.OrderBy(item => (int)item.scrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.createdDate).ToList();
            sortedScrapingQueueItemsWithoutCompletedScrapes = sortedScrapingQueueItemsWithoutCompletedScrapes.OrderBy(item => item.scrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.createdDate).ToList();
 

            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
            {
                return new List<ScrapingObject>();
            }

            return sortedScrapingQueueItemsWithoutCompletedScrapes;
        }

        public int getNumberOfThreadsAvailableOnServer()
        {
            //return maxAllowedServerScrapes - currentNumberOfThreadsPerBillingCompany.Skip(1).Sum(v => v.Value);
            return maxAllowedServerScrapes - currentNumberOfThreadsPerBillingCompany.Sum(v => v.Value);

        }

        public OpenClosedWindowDto GetCurrentOpenClosedWindowDtoByBillingCompany(Guid billingCompanyId)
        {
            OpenClosedWindowDto currentOpenClosedWindowDto = billingCompanyOpenClosedWindowsQuery.GetCurrentOpenClosedWindow(billingCompanyId);
            if (currentOpenClosedWindowDto == null)
            {
                return new OpenClosedWindowDto();
            }
            else
            {
                return currentOpenClosedWindowDto;
                
            }
        }

        public int getMaxAllowedThreadsForSpecificBillingCompany(OpenClosedWindowDto openClosedWindowDto, Guid billingCompanyId)
        {
            if (openClosedWindowDto != null)
            {
                if (!openClosedWindowDto.IsOpen)
                {
                    return 0;
                }
                else
                    return openClosedWindowDto.ConcurrentScrapingLimit;
            }
            else
                return getMaximumConcurrentThreadsAllowedForBillingCompanyAsDefinedByTheScrapingLoadManagementConfiguration(billingCompanyId);
        }

        public int NumberOfThreadsCurrentlyUsedByBillingCompanyId(Guid billingCompanyId)
        {
            int ThreadsCurrentlyUsedByBillingCompany = 0;

            if (currentNumberOfThreadsPerBillingCompany.ContainsKey(billingCompanyId))
                ThreadsCurrentlyUsedByBillingCompany = currentNumberOfThreadsPerBillingCompany[billingCompanyId];
            else
                currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 0);

            return ThreadsCurrentlyUsedByBillingCompany;

        }

        public void RescheduleItem(ScrapingObject scrapingQueueElement, DateTime EndDate)
        {
            scrapingQueueElement.ScheduledDate = EndDate;
        }

        public int getMaximumConcurrentThreadsAllowedForBillingCompanyAsDefinedByTheScrapingLoadManagementConfiguration(Guid billingCompanyId)
        {
            BillingCompanyScrapingLoadManagementConfigurationDto billingCompanyScrapingLoadManagementConfigurationDto = billingCompanyScrapingLoadManagementConfigurationQuery.GetBillingCompanyScrapingLoadManagementConfigurationById(billingCompanyId);
            return billingCompanyScrapingLoadManagementConfigurationDto.ConcurrentScrapes;
        }


        /*
        public void Stop()
        {

        }
        */

        /*
         * public List<ScrapingQueueElement> getNewScrapeQueueWithoutCompletedItems()
        { 
            
        }
         * */

        /*
        // This Handle's name will need to change to "CustomerBillingCompanyAccountAdded"
        public void Handle(CustomerBillingAccountAdded message)
        {
            // Store item in scrapingRepo
            ScrapingQueueElement scrapingQueueElement = scrapingObjectRepositoryFake.BuildNewScrapingObject(message.CustomerId, message.BillingCompanyId, message.RegistrationRequest);
            scrapingObjectRepositoryFake.StoreScrapingObject(scrapingQueueElement);
        }
        
         */
        public void mockAddBillingCompanyAccountAdded(ScrapingObject scrapingObject)
        {
            scrapingObjectRepositoryFake.StoreScrapingObject(scrapingObject);
        }

        public List<ScrapingObject> MockGetAllScrapingObjects()
        {
            return scrapingObjectRepositoryFake.GetAllScrapingObjects().Where(item => item.ScheduledDate <= DateTime.UtcNow).ToList();
        }
        // This Handle's name will need to change to "CustomerScrapeSessionFailed"
        public void Handle(ScrapeSessionFailed message)
        {
            // Look at error code and decide what to do with error
            // For all error types do the following:
            // currentNumberOfThreadsPerBillingCompany[scrapingQueueElement.billingCompanyId] -= 1;
            // scrapeElementsRunning.Remove(ScrapingQueueElement scrapingObject);

            /*
             *          Unknown = 0,
                        [Description("Invalid Credentials")] InvalidCredentials = 1 
                        [Description("Customer Not Signed Up for e-Billing")]  CustomerNotSignedUpForEBilling = 2,
                        [Description("Action Required by Billing Company’s Website")] ActionRequiredbyBillingCompanyWebsite = 3,
             *          [Description("Broken Script - Site Changed")] BrokenScript = 6
             *          [Description("Broken Script - Unhandled data condition")] BrokenScript = 7 
             *          ---> get scrapingObject from parameter.
             *          ---> call AddScrapingItemToCompletedQueue(ScrapingQueueElement scrapingObject)
             *          ---> scrapingObjectRepositoryFake.RemoveScrapingItemFromRepo(ScrapingQueueElement scrapingObject);
             
             
                        [Description("Billing Company’s Site Down")] BillingCompanySiteDown = 4
             *          ---> get scrapingObject from parameter.
             *          ---> get retryInterval for that BillingCompany -> ScrapingErrorRetryConfigurationDto(EnumType, HourDelay)
             *          ---> call RescheduleItem(scrapingQueueElement, DateTime.UtcNow.AddHours(6))
             *          
             * 
                        [Description("Error Page Encountered")] ErrorPageEncountered = 5,
             *          ---> get scrapingObject from parameter.
             *          ---> get retryInterval for that BillingCompany -> ScrapingErrorRetryConfigurationDto(EnumType, HourDelay)
             *          ---> call RescheduleItem(scrapingQueueElement, DateTime.UtcNow.AddHours(8))
                        
   
             * 
             * 
             */
        }

        /*
        
        public void Handle(AccountStatementGenerated message){}
        public void Handle(CustomerBillingCompanyAccountUpdated message){}
        public void Handle(CustomerBillingCompanyAccountDeleted message){}
        public void Handle(ScrapeSessionCompleted message){}
        public void Handle(ScrapeSessionDuplicateStatemsnt message){}
        
    */

    }
}