using Aps.Scraping.Scrapers;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Fakes
{
    public class CrossCheckScraperFake : ICrossCheckScraper
    {
        public bool CrossCheck(string url, string username, string password, string accountNumber)
        {
            Guard.That(url).IsNotNullOrEmpty().IsTrue(x => Uri.IsWellFormedUriString(url, UriKind.Absolute), "Invalid url");
            Guard.That(username).IsNotNullOrEmpty();
            Guard.That(password).IsNotNullOrEmpty();
            Guard.That(accountNumber).IsNotNullOrEmpty();
            return true;
        }
    }
}
