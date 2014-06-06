using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.EnumTypes
{
    public enum ScrapeSessionType
    {
        [Description("Cross Check Scrapper")]
        CrossCheckScrapper = 1,
        [Description("Statement Scrapper")]
        StatementScrapper = 2
    }
}
