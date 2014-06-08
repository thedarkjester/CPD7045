using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.Interpreters
{
    public class ScrapeSessionConversionException : Exception
    {
        public ScrapeSessionConversionException()
        {

        }

        public ScrapeSessionConversionException(string message)
        : base(message)
        {

        }

        public ScrapeSessionConversionException(string message, Exception inner)
        : base(message, inner)
        {

        }

        public ErrorCode ErrorCode { get; set; }
    }
}
