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
    public class SchedulingEngine : IHandle<ScrapeSessionFailed>, IHandle<CrossCheckCompleted>, IHandle<ScrapeSessionDuplicateStatement>, IHandle<ScrapeSessionSuccessfull>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly EventIntegrationService messageSendAndReceiver;
        private BillingCompanyOpenClosedWindowsQuery billingCompanyOpenClosedWindowsQuery;
        private IScrapingObjectRepository scrapingObjectRepositoryFake;
        private BillingCompanyBillingLifeCycleByCompanyIdQuery billingCompanyBillingLifeCycleByCompanyIdQuery;
        private BillingCompanyScrapingLoadManagementConfigurationQuery billingCompanyScrapingLoadManagementConfigurationQuery;
        private BillingCompanyCrossCheckEnabledByIdQuery billingCompanyCrossCheckEnabledByIdQuery;
        private IEnumerable<ScrapingObject> scrapeElementsQueue;
        private List<ScrapingObject> scrapeElementsRunning;
        private ScrapingObjectCreator scrapingObjectCreator;
        private ScrapeSessionInitiator scrapeSessionInitiator;
        private ScrapingErrorRetryConfigurationQuery scrapingErrorRetryConfigurationQuery;
        private int maxAllowedServerScrapes;
        private Dictionary<Guid, int> currentNumberOfThreadsPerBillingCompany;

        public SchedulingEngine(IEventAggregator eventAggregator, EventIntegrationService messageSendAndReceiver, IScrapingObjectRepository scrapingObjectRepositoryFake, BillingCompanyOpenClosedWindowsQuery billingCompanyOpenClosedWindowsQuery, BillingCompanyScrapingLoadManagementConfigurationQuery billingCompanyScrapingLoadManagementConfigurationQuery, ScrapeSessionInitiator scrapeSessionInitiator, ScrapingErrorRetryConfigurationQuery scrapingErrorRetryConfigurationQuery, ScrapingObjectCreator scrapingObjectCreator, BillingCompanyCrossCheckEnabledByIdQuery billingCompanyCrossCheckEnabledByIdQuery, BillingCompanyBillingLifeCycleByCompanyIdQuery billingCompanyBillingLifeCycleByCompanyIdQuery)  
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.billingCompanyOpenClosedWindowsQuery = billingCompanyOpenClosedWindowsQuery;
            this.scrapingObjectRepositoryFake = scrapingObjectRepositoryFake;
            this.billingCompanyScrapingLoadManagementConfigurationQuery = billingCompanyScrapingLoadManagementConfigurationQuery;
            this.currentNumberOfThreadsPerBillingCompany = new Dictionary<Guid, int>();
            this.messageSendAndReceiver = messageSendAndReceiver;
            this.scrapeElementsQueue = new List<ScrapingObject>();
            this.scrapeElementsRunning = new List<ScrapingObject>();
            this.scrapeSessionInitiator = scrapeSessionInitiator;
            this.scrapingErrorRetryConfigurationQuery = scrapingErrorRetryConfigurationQuery;
            this.scrapingObjectCreator = scrapingObjectCreator;
            this.billingCompanyCrossCheckEnabledByIdQuery = billingCompanyCrossCheckEnabledByIdQuery;
            this.billingCompanyBillingLifeCycleByCompanyIdQuery = billingCompanyBillingLifeCycleByCompanyIdQuery;
            maxAllowedServerScrapes = 20;
        }


        public void Start()
        {
            Scrape();
        }

        private void registerExternalEventsListeners()
        {
            messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(CustomerBillingAccountAdded).FullName);
            messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(ScrapingScriptUpdated).FullName);
            messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(BillingAccountDeletedFromCustomer).FullName);
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
                        scrapeSessionInitiator.InitiateNewScrapeSession(scrapingQueueElement); // Give ScrapeElement to ScrapeInitiator to start Scrape
                    }
                }
                Thread.Sleep(6000); // Entire QueueProcessed or threads full, sleep a while and then start with a new Queue
            }
        }

        public List<ScrapingObject> getNewScrapeQueueWithoutCompletedItems()
        //private List<ScrapingObject> getNewScrapeQueueWithoutCompletedItems()
        {
            List<ScrapingObject> sortedScrapingQueueItemsWithoutCompletedScrapes = scrapingObjectRepositoryFake.GetAllScrapingObjects().Where(item => item.ScheduledDate <= DateTime.UtcNow).ToList();
            List<ScrapingObject> completedScrapingQueue = scrapingObjectRepositoryFake.GetCompletedScrapeQueue().ToList();

            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
                return new List<ScrapingObject>();

            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => completedScrapingQueue.Any(y => y.queueId == x.queueId));
            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => scrapeElementsRunning.Any(y => y.queueId == x.queueId));
            scrapingObjectRepositoryFake.ClearCompletedScrapeList();
            sortedScrapingQueueItemsWithoutCompletedScrapes = sortedScrapingQueueItemsWithoutCompletedScrapes.OrderBy(item => item.scrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.createdDate).ToList();
 

            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
                return new List<ScrapingObject>();

            return sortedScrapingQueueItemsWithoutCompletedScrapes;
        }

        public int GetMaxNumberServerScrapesAllowed()
        {   
            return maxAllowedServerScrapes;
        }

        public int getNumberOfThreadsAvailableOnServer()
        //private int getNumberOfThreadsAvailableOnServer()
        {
            return maxAllowedServerScrapes - currentNumberOfThreadsPerBillingCompany.Sum(v => v.Value);
        }

        public OpenClosedWindowDto GetCurrentOpenClosedWindowDtoByBillingCompany(Guid billingCompanyId)
        //private OpenClosedWindowDto GetCurrentOpenClosedWindowDtoByBillingCompany(Guid billingCompanyId)
        {
            OpenClosedWindowDto currentOpenClosedWindowDto;
            //OpenClosedWindowDto tempCurrentOpenClosedWindowDto = null;
            try
            {
                currentOpenClosedWindowDto = billingCompanyOpenClosedWindowsQuery.GetCurrentOpenClosedWindow(billingCompanyId);
            }
            catch (Exception)
            {
                currentOpenClosedWindowDto = null;
            }

            return currentOpenClosedWindowDto;
        }

        public int getMaxAllowedThreadsForSpecificBillingCompany(OpenClosedWindowDto openClosedWindowDto, Guid billingCompanyId)
        //private int getMaxAllowedThreadsForSpecificBillingCompany(OpenClosedWindowDto openClosedWindowDto, Guid billingCompanyId)
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
        //private int NumberOfThreadsCurrentlyUsedByBillingCompanyId(Guid billingCompanyId)
        {
            int ThreadsCurrentlyUsedByBillingCompany = 0;

            if (currentNumberOfThreadsPerBillingCompany.ContainsKey(billingCompanyId))
                ThreadsCurrentlyUsedByBillingCompany = currentNumberOfThreadsPerBillingCompany[billingCompanyId];
            else
                currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 0);

            return ThreadsCurrentlyUsedByBillingCompany;

        }


        public void IncreaseNumberOfThreadsUsedByCompany(Guid billingCompanyId)
        //private void IncreaseNumberOfThreadsUsedByCompany(Guid billingCompanyId)
        {
            if (currentNumberOfThreadsPerBillingCompany.ContainsKey(billingCompanyId))
                currentNumberOfThreadsPerBillingCompany[billingCompanyId] += 1;
            else
                currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 1);
        }

        public void DecreaseNumberOfThreadsUsedByCompany(Guid billingCompanyId)
        //private void DecreaseNumberOfThreadsUsedByCompany(Guid billingCompanyId)
        {
            if (currentNumberOfThreadsPerBillingCompany.ContainsKey(billingCompanyId))
                currentNumberOfThreadsPerBillingCompany[billingCompanyId] -= 1;
            else
                currentNumberOfThreadsPerBillingCompany.Add(billingCompanyId, 0);
        }

        public void RescheduleItem(ScrapingObject scrapingQueueElement, DateTime EndDate)
        //private void RescheduleItem(ScrapingObject scrapingQueueElement, DateTime EndDate)
        {
            scrapingQueueElement.ScheduledDate = EndDate;
        }

        public int getMaximumConcurrentThreadsAllowedForBillingCompanyAsDefinedByTheScrapingLoadManagementConfiguration(Guid billingCompanyId)
        //private int getMaximumConcurrentThreadsAllowedForBillingCompanyAsDefinedByTheScrapingLoadManagementConfiguration(Guid billingCompanyId)
        {
            BillingCompanyScrapingLoadManagementConfigurationDto billingCompanyScrapingLoadManagementConfigurationDto;
            int numberOfConcurrentSessionsAllowed = 0;
            try
            {
                billingCompanyScrapingLoadManagementConfigurationDto = billingCompanyScrapingLoadManagementConfigurationQuery.GetBillingCompanyScrapingLoadManagementConfigurationById(billingCompanyId);
                numberOfConcurrentSessionsAllowed = billingCompanyScrapingLoadManagementConfigurationDto.ConcurrentScrapes;
            }
            catch (Exception)
            {
                numberOfConcurrentSessionsAllowed = 5; // Default value for number of ConcurrentScrapes allowed per billing Company is 5.
            }
            return numberOfConcurrentSessionsAllowed;
        }

        public IScrapingObjectRepository GetScrapingObjectRepositoryFake()
        {
            return this.scrapingObjectRepositoryFake;
        }

        public void AddScrapingElementToElementsRunningList(ScrapingObject scrapingObject)
        {
            scrapeElementsRunning.Add(scrapingObject);
        }

        public List<ScrapingObject> GetAllScrapeElementsRunningList()
        {
            return scrapeElementsRunning;
        }

        // Event - Internal
        public void Handle(ScrapeSessionFailed message)
        {
            ScrapingErrorRetryConfigurationDto retryDto;
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
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
                    try
                    {
                        retryDto = scrapingErrorRetryConfigurationQuery.GetAllScrapingErrorRetryConfigurations(scrapingObject.billingCompanyId).FirstOrDefault(x => (x.ResponseCode == ScrapingErrorResponseCodes.BillingCompanySiteDown));
                        RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(retryDto.RetryInterval));
                    }
                    catch (Exception)
                    {
                        RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(12)); // default time added is 12 hours if no dto is found
                    }
                break;

                case ScrapingErrorResponseCodes.ErrorPageEncountered:
                try
                {
                    retryDto = scrapingErrorRetryConfigurationQuery.GetAllScrapingErrorRetryConfigurations(scrapingObject.billingCompanyId).FirstOrDefault(x => (x.ResponseCode == ScrapingErrorResponseCodes.ErrorPageEncountered));
                    RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(retryDto.RetryInterval));
                }
                catch (Exception)
                { 
                    RescheduleItem(scrapingObject, DateTime.UtcNow.AddHours(6)); // default time added is 6 hours if no dto is found
                }
                break;

            }
        }

        // Event - Internal
        public void Handle(CrossCheckCompleted message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
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

        // Event - Internal
        public void Handle(ScrapeSessionSuccessfull message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
            scrapeElementsRunning.Remove(scrapingObject);
            scrapingObjectRepositoryFake.AddScrapingItemToCompletedQueue(scrapingObject);

            BillingCompanyBillingLifeCycleDto dto = billingCompanyBillingLifeCycleByCompanyIdQuery.GetBillingCompanyBillingLifeCycleByCompanyId(scrapingObject.billingCompanyId);
            DateTime dateTime;

            if (dto == null)
                dateTime = message.StatementDate.AddDays(28); // if no life-cycle parameters are found reschedule scrape for statement date + 28 days
            else 
            dateTime = message.StatementDate.AddDays(dto.DaysPerBillingCycle).AddDays(-1 * dto.LeadTimeInterval);

            RescheduleItem(scrapingObject, dateTime);
            
        }

        // Event - Internal
        public void Handle(ScrapeSessionDuplicateStatement message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByQueueId(message.QueueId);
            DecreaseNumberOfThreadsUsedByCompany(scrapingObject.billingCompanyId);
            scrapeElementsRunning.Remove(scrapingObject);
            scrapingObjectRepositoryFake.AddScrapingItemToCompletedQueue(scrapingObject);

            BillingCompanyBillingLifeCycleDto dto = billingCompanyBillingLifeCycleByCompanyIdQuery.GetBillingCompanyBillingLifeCycleByCompanyId(scrapingObject.billingCompanyId);
            DateTime dateTime;

            if (dto == null)
                dateTime = DateTime.UtcNow.AddDays(2); // default time added is 2 days if no dto is found
            else 
            dateTime = DateTime.UtcNow.AddDays(dto.RetryInterval);

            RescheduleItem(scrapingObject, dateTime);
        }

        // Events - External;     
        public void Handle(BillingAccountDeletedFromCustomer message)
        {
            ScrapingObject scrapingObject = scrapingObjectRepositoryFake.GetScrapingObjectByCustomerAndBillingCompanyId(message.CustomerId, message.BillingCompanyId);
            scrapingObjectRepositoryFake.RemoveScrapingItemFromRepo(scrapingObject);
        }  
         
        // Event - External
        public void Handle(CustomerBillingAccountAdded message)
         {
             ScrapeSessionTypes crossCheckEnabled;
             BillingCompanyCrossCheckDto dto;
             try
             {
                 dto = billingCompanyCrossCheckEnabledByIdQuery.GetBillingCompanyCrossCheckEnabledById(message.billingCompanyId);
                 bool crossCheckEnabledFlag = dto.crossCheckScrapeEnabled;

                 if (crossCheckEnabledFlag)
                     crossCheckEnabled = ScrapeSessionTypes.CrossCheckScrapper;
                 else
                     crossCheckEnabled = ScrapeSessionTypes.StatementScrapper;
             }
             catch (Exception)
             {
                 crossCheckEnabled = ScrapeSessionTypes.StatementScrapper;
             }

             ScrapingObject scrapingObject = scrapingObjectCreator.GetNewScrapingObject(message.customerId, message.billingCompanyId, crossCheckEnabled);
             scrapingObjectRepositoryFake.StoreScrapingObject(scrapingObject);
         }

        // External Event
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