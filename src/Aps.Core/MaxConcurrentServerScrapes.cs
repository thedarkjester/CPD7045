using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService
{
    

    public class MaxConcurrentServerScrapes
    {
        private int maxAllowedServerScrapes;

        public MaxConcurrentServerScrapes(int startupValue)
        {
            maxAllowedServerScrapes = startupValue;
        }

        public int GetMaxAllowedServerScrapes()
        {
            return maxAllowedServerScrapes;
        }

        public void SetMaxAllowedServerScrapes(int maxAllowedServerScrapes)
        {
            this.maxAllowedServerScrapes = maxAllowedServerScrapes;
        }
    }
}
