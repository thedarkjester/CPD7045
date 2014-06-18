using Aps.Scraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService
{
    public interface IScrapeQueueingStrategy
    {
        IEnumerable<ScrapingObject> GetQueue(List<ScrapingObject> runningElements, List<ScrapingObject> completedElements, List<ScrapingObject> allElements);
    }
}
