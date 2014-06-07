using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aps.Core.Interpreters
{
    public class ScrapeNoDataPairsFoundException : Exception
    {
        public ScrapeNoDataPairsFoundException()
        {

        }

        public ScrapeNoDataPairsFoundException(string message)
        : base(message)
        {

        }

        public ScrapeNoDataPairsFoundException(string message, Exception inner)
        : base(message, inner)
        {

        }

        public ErrorCode ErrorCode { get; set; }
    }
}
