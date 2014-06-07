using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.InternalEvents
{
    public class CrossCheckCompleted
    {
        Guid queueId;
        public Guid QueueId { get { return queueId; } } 
        public CrossCheckCompleted(Guid queueId)
        {
            this.queueId = queueId;
        }
    }
}
