using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aps.Core;
using Aps.Core.Interpreters;

namespace Aps.Shared.Tests.InterpreterTests
{
    [TestClass]
    public class ScrapeSessionDataInterpreterTests
    {
        [TestMethod]
        public void WhenReceicingValidXmlWith10DataPairsScrapeSessionDataPairsShouldBeReturned()
        {
            string xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><text>Account no</text><value>53844946068883</value></datapair><datapair id=""002""><text>Service ref</text><value>0117838898</value></datapair><datapair id=""003""><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=""004""><text>Payment</text><value>R513.00</value></datapair><datapair id=""005""><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>";
            Interpreter interpreter = new Interpreter();
            var dataPairs = interpreter.TransformResults(xml);
            Assert.AreEqual(10, dataPairs.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ScrapeNoDataPairsFoundException), "There were no data pairs in the xml")]
        public void WhenReceicingValidXmlWithNoDataPairsScrapeNoDataPairsFoundExceptionShouldBeReturned()
        {
            string xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time></scrape-session>";
            Interpreter interpreter = new Interpreter();
            var dataPairs = interpreter.TransformResults(xml);

        }

        [TestMethod]
        public void WhenReceicingValidXmlWithDataPairWithNoNameScrapeSessionDataPairsShouldBeReturned()
        {
            string xml = @"<scrape-session><base-url>www.telkom.co.za</base-url><date>10/01/2008</date><time>13:50:00</time><datapair id=""001""><value>53844946068883</value></datapair><datapair id=""002""><text>Service ref</text><value>0117838898</value></datapair><datapair id=""003""><text>Previous Invoice</text><value>R512.22</value></datapair><datapair id=""004""><text>Payment</text><value>R513.00</value></datapair><datapair id=""005""><text>Opening Balance</text><value>R0.78</value></datapair></scrape-session>";
            Interpreter interpreter = new Interpreter();
            var dataPairs = interpreter.TransformResults(xml);
            Assert.AreEqual(9, dataPairs.Count);

        }
    }


}
