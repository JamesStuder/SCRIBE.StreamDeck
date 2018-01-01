using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCRIBE.StreamDeck.Data_Access_Objects;
using SCRIBE.StreamDeck.Models;

namespace SCRIBE.StreamDeck.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void GetFunctionsUnitTest()
        {
            XmlDAO xml = new XmlDAO();
            string xmlDoc = @"Data\SCRIBE.Functions.xml";
            List<FunctionModel> oFunctions = xml.GetFunctions("Conversion", xmlDoc, null);
            Assert.IsNotNull(oFunctions);
            int funcCount = oFunctions.Count;
            Assert.AreEqual(10, funcCount);
        }

        [TestMethod]
        public void OpenXmlFile()
        {
            XmlDAO xml = new XmlDAO();
            string xmlDoc = @"Data\SCRIBE.Functions.xml";
            xml.OpenXmlFile(xmlDoc, null);
        }
    }
}
