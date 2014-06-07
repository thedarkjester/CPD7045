using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Aps.Core.Interpreters;

namespace Aps.Core
{
    public class Interpreter
    {
        public IList<DataPair> TransformResults(string scrapeSessionXmlData)
        {
            XDocument doc = XDocument.Parse(scrapeSessionXmlData);
            IList<DataPair> dataPairs = new List<DataPair>();
            var xMembers = doc.Root.Elements("datapair").Elements().ToList();
            if(xMembers.Count.Equals(0))
            {
                ScrapeNoDataPairsFoundException exception = new ScrapeNoDataPairsFoundException();
                exception.ErrorCode = ErrorCode.NoDataPairs;
                throw exception;
            }
            LoadDataPairs(dataPairs, xMembers);
            return dataPairs;
        }

        private void LoadDataPairs(IList<DataPair> dataPairs, List<XElement> xMembers)
        {
            foreach (XElement x in xMembers)
            {
                DataPair dataPair = new DataPair();
                dataPair.Name = x.Name.ToString();
                dataPair.Value = x.Value;
                dataPairs.Add(dataPair);
            }
        }
    }
}
