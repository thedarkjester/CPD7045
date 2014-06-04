using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core
{
    public class ScrapeSessionData
    {
        public string Url { get; set; }

        public DateTime Date { get; set; }

        public IList<DataPair> MyProperty { get; set; }
    }
}
