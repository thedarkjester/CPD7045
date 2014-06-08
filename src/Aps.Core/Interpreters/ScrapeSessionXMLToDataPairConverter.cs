using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Aps.Scheduling.ApplicationService.Interpreters;

namespace Aps.Scheduling.ApplicationService
{
    public class ScrapeSessionXMLToDataPairConverter
    {
        public IList<InterpretedScrapeSessionDataPair> ConvertXmlToScrapeSessionDataPairs(string scrapeSessionXmlData)
        {
            XDocument doc = XDocument.Parse(scrapeSessionXmlData);
            IList<InterpretedScrapeSessionDataPair> dataPairs = new List<InterpretedScrapeSessionDataPair>();
            var xMembers = doc.Root.Elements("datapair").Elements().ToList();
            if (xMembers.Count.Equals(0))
            {
                ScrapeSessionConversionException exception = new ScrapeSessionConversionException();
                exception.ErrorCode = ErrorCode.NoDataPairs;
                throw exception;
            }
            LoadDataPairs(dataPairs, xMembers);
            return dataPairs;
        }

        private void LoadDataPairs(IList<InterpretedScrapeSessionDataPair> dataPairs, List<XElement> xMembers)
        {
            string key = string.Empty;
            InterpretedScrapeSessionDataPair dataPair = null;
            foreach (XElement x in xMembers)
            {
                if (x.Name.ToString().Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    GetKey(ref key, ref dataPair, x);
                }
                else
                {
                    GetValue(dataPairs, key, dataPair, x);
                }
            }
        }

        private static void GetValue(IList<InterpretedScrapeSessionDataPair> dataPairs, string key, InterpretedScrapeSessionDataPair dataPair, XElement x)
        {
            if (dataPair != null)
            {
                dataPair.keyValuePair = new KeyValuePair<string, string>(key, x.Value);
                dataPairs.Add(dataPair);
            }
        }

        private static void GetKey(ref string key, ref InterpretedScrapeSessionDataPair dataPair, XElement x)
        {
            dataPair = new InterpretedScrapeSessionDataPair();
            key = x.Value;
        }
    }
}
