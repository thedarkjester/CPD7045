using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aps.Scheduling.ApplicationService
{
    public class InterpretedScrapeSessionDataPair
    {
        public int Id { get; set; }

        public KeyValuePair<string, object> keyValuePair { get; set; }

    }
}
