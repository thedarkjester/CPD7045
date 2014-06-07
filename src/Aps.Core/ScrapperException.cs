using Aps.Integration.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core
{
    public class ScrapperException : Exception
    {
        private readonly ScrapingErrorResponseCodes scrapingErrorResponseCodes;

        public ScrapingErrorResponseCodes Error
        {
            get { return scrapingErrorResponseCodes; }
        }

        public ScrapperException(ScrapingErrorResponseCodes scrapingErrorResponseCodes)
        {
            this.scrapingErrorResponseCodes = scrapingErrorResponseCodes;
        }
    }
}
