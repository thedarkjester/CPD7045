using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.InternalEvents
{
    public class ScrapeSessionDuplicateStatement
    {
        Guid queueId;
        public Guid QueueId { get { return queueId; } }
        public ScrapeSessionDuplicateStatement(Guid queueId)
        {
            this.queueId = queueId;
        }
    }
}
