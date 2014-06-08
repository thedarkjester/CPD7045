using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.Validation
{
    public class ScrapeValidationException: Exception
    {
        public ScrapeValidationException()
        {

        }

        public ScrapeValidationException(string message)
        : base(message)
        {

        }

        public ScrapeValidationException(string message, Exception inner)
        : base(message, inner)
        {

        }

        public ErrorCode ErrorCode { get; set; }
    }
}
