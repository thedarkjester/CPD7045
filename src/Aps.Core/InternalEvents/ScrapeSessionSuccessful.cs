using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.InternalEvents
{
    public class ScrapeSessionSuccessfull
    {
        Guid queueId;
        DateTime statementDate;

        public Guid QueueId { get { return queueId; } }
        public DateTime StatementDate { get { return statementDate; } }

        public ScrapeSessionSuccessfull(Guid queueId, DateTime statementDate)
        {
            this.queueId = queueId;
            this.statementDate = statementDate;
        }
    }
}
