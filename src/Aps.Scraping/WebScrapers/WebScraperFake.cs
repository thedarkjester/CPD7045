using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scraping.WebScrapers
{
    public class WebScraperFake : IWebScraper
    {
        public string Scrape(string url, string username, string password)
        {
            Guard.That(url).IsNotNullOrEmpty().IsTrue(x => Uri.IsWellFormedUriString(url, UriKind.Absolute), "Invalid url");
            Guard.That(username).IsNotNullOrEmpty();
            Guard.That(password).IsNotNullOrEmpty();
            return @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=001><text>Account no</text><value>53844946068883</value></datapair><datapair id=002><text>Service ref</text><value>0117838898</value></datapair><datapair id=003><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=004><text>Payment</text><value>R513.00</value></datapair><datapair id=005><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>";
        }
    }
}
