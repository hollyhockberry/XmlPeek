// Copyright (c) 2023 @hollyhockberry
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Xml.Linq;

namespace XmlPicker.UnitTest
{
    [TestClass]
    public class AttributeTest
    {
        [TestMethod]
        public void TestAttributedElement()
        {
            var xml = new XElement("Root", new XElement("Element", new XAttribute("Attr", "Value")));
            var element = new Element(xml, "Element");
            Assert.AreEqual("Value", element.XElement?.Attribute("Attr")?.Value);
            element.Poke(xml);
            Assert.AreEqual("Value", xml.Element("Element")?.Attribute("Attr")?.Value);
        }

        [TestMethod]
        public void TestAttributedElementList()
        {
            var xml = new XElement("Root",
                new XElement("ElementList", new object[]
                {
                    new XAttribute("Attr", "Value"),
                    new XElement("Element", new XElement("Child", "0")),
                    new XElement("Element", new XElement("Child", "1")),
                    new XElement("Element", new XElement("Child", "2")),
                    new XElement("Element", new XElement("Child", "3")),
                    new XElement("Element", new XElement("Child", "4")),
                }));

            var element = new ElementList<Element>("Element", xml, "ElementList");

            Assert.AreEqual("Value", element.XElement?.Attribute("Attr")?.Value);
            element.Poke(xml);
            Assert.AreEqual("Value", xml.Element("ElementList")?.Attribute("Attr")?.Value);
        }

    }
}
