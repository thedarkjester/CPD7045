using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.InternalEvents
{
    public class CrossCheckCompleted
    {
        Guid queueId;
        bool successful;
        public Guid QueueId { get { return queueId; } }
        public bool Successful { get { return successful; } } 

        public CrossCheckCompleted(Guid queueId, bool successful)
        {
            this.queueId = queueId;
            this.successful = successful;
        }
    }
}
