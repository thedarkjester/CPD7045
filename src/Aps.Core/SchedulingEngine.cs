using Aps.BillingCompanies;
using Aps.Customers;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Scheduling.ApplicationService.InternalEvents;
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
using Aps.Integration.EnumTypes;

namespace Aps.Scheduling.ApplicationService
{
    public class SchedulingEngine : IHandle<ScrapeSessionFailed>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly EventIntegrationService messageSendAndReceiver;
        private BillingCompanyOpenClosedWindowsQuery billingCompanyOpenClosedWindowsQuery;
        private IScrapingObjectRepository scrapingObjectRepositoryFake;
        private BillingCompanyBillingLifeCycleByCompanyIdQuery billingCompanyBillingLifeCycleByCompanyIdQuery;
        private BillingCompanyScrapingLoadManagementConfigurationQuery billingCompanyScrapingLoadManagementConfigurationQuery;
        private BillingCompanyCrossCheckEnabledByIdQuery billingCompanyCrossCheckEnabledByIdQuery;
        public Dictionary<Guid, int> currentNumberOfThreadsPerBillingCompany;
        public int maxAllowedServerScrapes;
        public IEnumerable<ScrapingObject> scrapeElementsQueue;
        public List<ScrapingObject> scrapeElementsRunning;
        public CancellationToken cancellationToken;
        //readonly ScrapeSessionInitiator scrapeSessionInitiator; // Add this when Jignesh's stuff is done.
        readonly ScrapeSessionInitiatorFake scrapeSessionInitiator;

        readonly ScrapingErrorRetryConfigurationQuery scrapingErrorRetryConfigurationQuery;
        public ScrapingObjectCreator scrapingObjectCreator;

        public SchedulingEngine(IEventAggregator eventAggregator, EventIntegrationService messageSendAndReceiver, IScrapingObjectRepository scrapingObjectRepositoryFake, BillingCompanyOpenClosedWindowsQuery billingCompanyOpenClosedWindowsQuery, BillingCompanyScrapingLoadManagementConfigurationQuery billingCompanyScrapingLoadManagementConfigurationQuery, ScrapeSessionInitiatorFake scrapeSessionInitiator, ScrapingErrorRetryConfigurationQuery scrapingErrorRetryConfigurationQuery, ScrapingObjectCreator scrapingObjectCreator, BillingCompanyCrossCheckEnabledByIdQuery billingCompanyCrossCheckEnabledByIdQuery, BillingCompanyBillingLifeCycleByCompanyIdQuery billingCompanyBillingLifeCycleByCompanyIdQuery)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.billingCompanyOpenClosedWindowsQuery = billingCompanyOpenClosedWindowsQuery;
            this.scrapingObjectRepositoryFake = scrapingObjectRepositoryFake;
            this.billingCompanyScrapingLoadManagementConfigurationQuery = billingCompanyScrapingLoadManagementConfigurationQuery;
            this.currentNumberOfThreadsPerBillingCompany = new Dictionary<Guid, int>();
            this.messageSendAndReceiver = messageSendAndReceiver;
            maxAllowedServerScrapes = 20;
            this.scrapeElementsQueue = new List<ScrapingObject>();
            this.scrapeElementsRunning = new List<ScrapingObject>();
            this.scrapeSessionInitiator = scrapeSessionInitiator;
            this.scrapingErrorRetryConfigurationQuery = scrapingErrorRetryConfigurationQuery;
            this.scrapingObjectCreator = scrapingObjectCreator;
            this.billingCompanyCrossCheckEnabledByIdQuery = billingCompanyCrossCheckEnabledByIdQuery;
            this.billingCompanyBillingLifeCycleByCompanyIdQuery = billingCompanyBillingLifeCycleByCompanyIdQuery;
        }


        public void Start()
        {
            //messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(NewCustomerBillingCompanyAccount).FullName);
            Scrape();
        }



        private void Scrape()
        {
            OpenClosedWindowDto openClosedWindowDto;
            int maxAllowedThreadsForSpecificBillingCompany = 0;

            while (true)
            {
                Console.WriteLine("Accepting EventAggregator Events");
                scrapeElementsQueue = getNewScrapeQueueWithoutCompletedItems();
                if (scrapeElementsQueue == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("No scrape sessions found, waiting 2 seconds");
                    Thread.Sleep(2000);
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
                        IncreaseNumberOfThreadsUsedByCompany(scrapingQueueElement.billingCompanyId);
                        scrapeElementsRunning.Add(scrapingQueueElement);
                        // This is where I pass the work to Jignesh! Once it exists, add it. 
                        //scrapeSessionInitiator.InitiateNewScrapeSession(scrapingQueueElement);

                    }
                }
                Thread.Sleep(6000); // Entire QueueProcessed or threads full, sleep a while and then start with a new Queue
            }
        }

        public List<ScrapingObject> getNewScrapeQueueWithoutCompletedItems()
        {
            List<ScrapingObject> sortedScrapingQueueItemsWithoutCompletedScrapes = scrapingObjectRepositoryFake.GetAllScrapingObjects().Where(item => item.ScheduledDate <= DateTime.UtcNow).ToList();
            List<ScrapingObject> completedScrapingQueue = scrapingObjectRepositoryFake.GetCompletedScrapeQueue().ToList();

            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
                return new List<ScrapingObject>();

            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => completedScrapingQueue.Any(y => y.queueId == x.queueId));
            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => scrapeElementsRunning.Any(y => y.queueId == x.queueId));
            scrapingObjectRepositoryFake.ClearCompletedScrapeList();
           // sortedScrapingQueueItemsWithoutCompletedScrapes = sortedScrapingQueueItemsWithoutCompletedScrapes.OrderBy(item => (int)item.scrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.createdDate).ToList();
            sortedScrapingQueueItemsWithoutCompletedScrapes = sortedScrapingQueueItemsWithoutCompletedScrapes.OrderBy(item => item.scrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.createdDate).ToList();
 

            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
                return new List<ScrapingObject>();

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
                return null;
            else return currentOpenClosedWindowDto;
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

        public void IncreaseNumberOfThreadsUsedByCompany(Guid billingCompanyId)
        {
            if (currentNumberOfThreadsPerBillingCompany.ContainsKey(billingCompanyId))
                currentNumberOfThreadsPerBillingCompany[billingCompanyId] += 1;
            else
                currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 1);
        }

        public void DecreaseNumberOfThreadsUsedByCompany(Guid billingCompanyId)
        {
            if (currentNumberOfThreadsPerBillingCompany.ContainsKey(billingCompanyId))
                currentNumberOfThreadsPerBillingCompany[billingCompanyId] -= 1;
            else
                currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 0);
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

        public void mockAddBillingCompanyAccountAdded(ScrapingObject scrapingObject)
        {
            scrapingObjectRepositoryFake.StoreScrapingObject(scrapingObject);
        }

        public List<ScrapingObject> MockGetAllScrapingObjectsScheduledInPast()
        {
            return scrapingObjectRepositoryFake.GetAllScrapingObjects().Where(item => item.ScheduledDate <= DateTime.UtcNow).ToList();
            //return scrapingObjectRepositoryFake.GetAllScrapingObjects().ToList();
        }

        public void Handle(ScrapeSessionFailed message)
        {
            ScrapingErrorRetryConfigurationDto retryDto;
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
            //currentNumberOfThreadsPerBillingCompany[scrapingObject.billingCompanyId] -= 1;
            scrapeElementsRunning.Remove(scrapingObject);

            switch (message.ScrapingErrorResponseCode)
            {

                case ScrapingErrorResponseCodes.Unknown:
                case ScrapingErrorResponseCodes.UnhandledDataCondition:
                case ScrapingErrorResponseCodes.BrokenScript:
                    scrapingObject.scrapeStatus = "inactive";
                    RescheduleItem(scrapingObject, DateTime.MaxValue);
                    break;

                case ScrapingErrorResponseCodes.InvalidCredentials:
                case ScrapingErrorResponseCodes.CustomerNotSignedUpForEBilling:
                case ScrapingErrorResponseCodes.ActionRequiredbyBillingCompanyWebsite:
                    scrapingObjectRepositoryFake.AddScrapingItemToCompletedQueue(scrapingObject);
                    scrapingObjectRepositoryFake.RemoveScrapingItemFromRepo(scrapingObject);
                    break;


                case ScrapingErrorResponseCodes.BillingCompanySiteDown:
                    retryDto = scrapingErrorRetryConfigurationQuery.GetAllScrapingErrorRetryConfigurations(scrapingObject.billingCompanyId).FirstOrDefault(x => (x.ResponseCode == ScrapingErrorResponseCodes.BillingCompanySiteDown));
                    RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(retryDto.RetryInterval));
                    //RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(12));
                    break;

                case ScrapingErrorResponseCodes.ErrorPageEncountered:
                    retryDto = scrapingErrorRetryConfigurationQuery.GetAllScrapingErrorRetryConfigurations(scrapingObject.billingCompanyId).FirstOrDefault(x => (x.ResponseCode == ScrapingErrorResponseCodes.ErrorPageEncountered));
                    RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(retryDto.RetryInterval));
                    //RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(6));
                    break;

            }
        }

        // Jignesh Event - Internal
        public void Handle(CrossCheckCompleted message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
            //currentNumberOfThreadsPerBillingCompany[scrapingObject.billingCompanyId] -= 1;
            scrapeElementsRunning.Remove(scrapingObject);

            if (message.Successful)
            {
                scrapingObject.scrapeSessionTypes = ScrapeSessionTypes.StatementScrapper;
                RescheduleItem(scrapingObject, DateTime.UtcNow);
            }
            else
            {
                scrapingObjectRepositoryFake.RemoveScrapingItemFromRepo(scrapingObject);
            }
        }

        // Jignesh Event - Internal
        public void Handle(ScrapeSessionSuccessful message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
            //currentNumberOfThreadsPerBillingCompany[scrapingObject.billingCompanyId] -= 1;
            scrapeElementsRunning.Remove(scrapingObject);
            scrapingObjectRepositoryFake.AddScrapingItemToCompletedQueue(scrapingObject);

            BillingCompanyBillingLifeCycleDto dto = billingCompanyBillingLifeCycleByCompanyIdQuery.GetBillingCompanyBillingLifeCycleByCompanyId(scrapingObject.billingCompanyId);

            DateTime dateTime = message.StatementDate.AddDays(dto.DaysPerBillingCycle).AddDays(-1 * dto.LeadTimeInterval);
            RescheduleItem(scrapingObject, dateTime);
            
        }

        // Jignesh Event - Internal
        public void Handle(ScrapeSessionDuplicateStatement message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
            //currentNumberOfThreadsPerBillingCompany[scrapingObject.billingCompanyId] -= 1;
            scrapeElementsRunning.Remove(scrapingObject);
            scrapingObjectRepositoryFake.AddScrapingItemToCompletedQueue(scrapingObject);

            BillingCompanyBillingLifeCycleDto dto = billingCompanyBillingLifeCycleByCompanyIdQuery.GetBillingCompanyBillingLifeCycleByCompanyId(scrapingObject.billingCompanyId);

            DateTime dateTime = DateTime.UtcNow.AddDays(dto.RetryInterval);
            RescheduleItem(scrapingObject, dateTime);
        }

        // Carlos Events - - External; Completed, but Carlos still to create class
        /*
        public void Handle(BillingAccountDeletedFromCustomer message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByCustomerAndBillingCompanyId(message.customerId, message.billingCompanyId);
            scrapingObjectRepositoryFake.RemoveScrapingItemFromRepo(scrapingObject);
        }  
         */

        // Carlos Event - External
         public void Handle(CustomerBillingAccountAdded message)
         {
             ScrapeSessionTypes crossCheckEnabled;
             BillingCompanyCrossCheckDto dto = billingCompanyCrossCheckEnabledByIdQuery.GetBillingCompanyCrossCheckEnabledById(message.billingCompanyId);
             bool crossCheckEnabledFlag = dto.crossCheckScrapeEnabled;

             if (crossCheckEnabledFlag)
                 crossCheckEnabled = ScrapeSessionTypes.CrossCheckScrapper;
             else
                 crossCheckEnabled = ScrapeSessionTypes.StatementScrapper;

             ScrapingObject scrapingObject = scrapingObjectCreator.GetNewScrapingObject(message.customerId, message.billingCompanyId, crossCheckEnabled);
             scrapingObjectRepositoryFake.StoreScrapingObject(scrapingObject);
         }

         public void Handle(ScrapingScriptUpdated message)
         {
             List<ScrapingObject> myList = scrapingObjectRepositoryFake.GetAllScrapingObjectsByBillingCompanyId(message.BillingId).ToList();
             foreach (var item in myList)
             {
                 if (item.scrapeStatus == "inactive")
                     RescheduleItem(item, DateTime.UtcNow);
             }
         }
    }
}