using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aps.Core;

namespace Aps.Shared.Tests.InterpreterTests
{
    [TestClass]
    public class ScrapeSessionDataInterpreterTests
    {
        [TestMethod]
        public void WhenReceicingValidXmlScrapeSessionDataPairsShouldBeReturned()
        {
            string xml = string.Empty;
            Interpreter interpreter = new Interpreter();
            var r = interpreter.TransformResults(xml);
        }
    }


}
