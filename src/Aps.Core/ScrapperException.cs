using Aps.Integration.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core
{
    public class DataScraperException : Exception
    {
        private readonly ScrapingErrorResponseCodes scrapingErrorResponseCodes;

        public ScrapingErrorResponseCodes Error
        {
            get { return scrapingErrorResponseCodes; }
        }

        public DataScraperException(ScrapingErrorResponseCodes scrapingErrorResponseCodes)
        {
            this.scrapingErrorResponseCodes = scrapingErrorResponseCodes;
        }
    }
}
