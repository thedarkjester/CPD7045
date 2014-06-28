using Aps.Scraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService
{
    public class ScrapeQueueingStrategyByRegistrationScheduledDateCreatedDate : IScrapeQueueingStrategy
    {
        public IEnumerable<ScrapingObject> GetQueue(List<ScrapingObject> runningElements, List<ScrapingObject> completedElements, List<ScrapingObject> allElements)
        {
            List<ScrapingObject> sortedScrapingQueueItemsWithoutCompletedScrapes = allElements.Where(item => item.ScheduledDate <= DateTime.UtcNow).ToList(); 
            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
                return new List<ScrapingObject>();

            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => completedElements.Any(y => y.QueueId == x.QueueId));
            sortedScrapingQueueItemsWithoutCompletedScrapes.RemoveAll(x => runningElements.Any(y => y.QueueId == x.QueueId));
            sortedScrapingQueueItemsWithoutCompletedScrapes = sortedScrapingQueueItemsWithoutCompletedScrapes.OrderBy(item => item.ScrapeSessionTypes).ThenBy(item => item.ScheduledDate).ThenBy(item => item.CreatedDate).ToList();


            if (sortedScrapingQueueItemsWithoutCompletedScrapes.Count() == 0)
                return new List<ScrapingObject>();

            return sortedScrapingQueueItemsWithoutCompletedScrapes;
        }
    }
}
