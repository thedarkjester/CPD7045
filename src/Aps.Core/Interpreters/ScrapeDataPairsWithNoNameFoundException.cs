using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.Interpreters
{
    public class ScrapeDataPairsWithNoNameFoundException : Exception
    {
        public ScrapeDataPairsWithNoNameFoundException()
        {

        }

        public ScrapeDataPairsWithNoNameFoundException(string message)
        : base(message)
        {

        }

        public ScrapeDataPairsWithNoNameFoundException(string message, Exception inner)
        : base(message, inner)
        {

        }

        public ErrorCode ErrorCode { get; set; }
    }
}
