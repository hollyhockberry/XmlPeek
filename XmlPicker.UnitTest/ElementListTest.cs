// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Linq;
using System.Xml.Linq;

namespace XmlPicker.UnitTest
{
    [TestClass]
    public class ElementListTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            var xml = new XElement("Root",
                new XElement("ElementList", new object[]
                {
                    new XElement("Element", new XElement("Child", "0")),
                    new XElement("Element", new XElement("Child", "1")),
                    new XElement("Element", new XElement("Child", "2")),
                    new XElement("Element", new XElement("Child", "3")),
                    new XElement("Element", new XElement("Child", "4")),
                }));

            var element = new ElementList<Element>("Element", xml, "ElementList");

            Assert.IsFalse(element.IsReadOnly);
            Assert.AreEqual(5, element.Count);
            Assert.AreEqual(1, element[0].XElement?.Elements()?.Count());
            Assert.AreEqual("0", element[0].XElement?.Element("Child")?.Value);
            Assert.AreEqual(1, element[1].XElement?.Elements()?.Count());
            Assert.AreEqual("1", element[1].XElement?.Element("Child")?.Value);
            Assert.AreEqual(1, element[2].XElement?.Elements()?.Count());
            Assert.AreEqual("2", element[2].XElement?.Element("Child")?.Value);
            Assert.AreEqual(1, element[3].XElement?.Elements()?.Count());
            Assert.AreEqual("3", element[3].XElement?.Element("Child")?.Value);
            Assert.AreEqual(1, element[4].XElement?.Elements()?.Count());
            Assert.AreEqual("4", element[4].XElement?.Element("Child")?.Value);

            Assert.AreEqual(1, xml.Elements().Count());
            Assert.AreEqual(0, xml.Element("ElementList")?.Elements()?.Count());

            element.Poke(xml);

            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(5, xml.Element("ElementList")?.Elements()?.Count());
            Assert.AreEqual("01234", string.Join("", xml.Element("ElementList")?.Elements()?.Select(e => e.Value)!));

            element.Poke(xml);
            Assert.AreEqual(5, xml.Element("ElementList")?.Elements()?.Count());
        }

        [TestMethod]
        public void TestEmptyConstructAndModify()
        {
            var xml = new XElement("Root", new XElement("ElementList"));
            var element = new ElementList<Element>("Element", xml, "ElementList");

            Assert.AreEqual(0, element.Count);
            Assert.AreEqual(1, xml.Elements().Count());
            Assert.AreEqual(0, xml.Element("ElementList")?.Elements()?.Count());

            element.Poke(xml);
            Assert.AreEqual(0, xml.Element("ElementList")?.Elements()?.Count());

            element.Add(new Element("Element"));
            element[0].ValidXElement.Add(new XElement("Child", "0"));
            Assert.AreEqual(1, element.Count);
            Assert.AreEqual(0, xml.Element("ElementList")?.Elements()?.Count());

            element.Poke(xml);
            Assert.AreEqual(1, xml.Element("ElementList")?.Elements()?.Count());
            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(1, xml.Element("ElementList")?.Elements()?.Count());
            Assert.AreEqual(1, xml.Element("ElementList")?.Element("Element")?.Elements()?.Count());
            Assert.AreEqual("0", xml.Element("ElementList")?.Element("Element")?.Element("Child")?.Value);

            element.Poke(xml);
            Assert.AreEqual(1, xml.Element("ElementList")?.Elements()?.Count());
        }

        [TestMethod]
        public void TestNullConstructAndModify()
        {
            var xml = new XElement("Root");
            var element = new ElementList<Element>("Element", xml, "ElementList");

            Assert.AreEqual(0, element.Count);
            Assert.IsNull(element.XElement);

            element.Poke(xml);
            Assert.IsNull(xml.Element("ElementList"));

            element.Add(new Element("Element"));
            element[0].ValidXElement.Add(new XElement("Child", "0"));
            Assert.AreEqual(1, element.Count);
            Assert.IsNull(xml.Element("ElementList"));

            element.Poke(xml);
            Assert.AreEqual(1, xml.Element("ElementList")?.Elements()?.Count());
            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(1, xml.Element("ElementList")?.Elements()?.Count());
            Assert.AreEqual(1, xml.Element("ElementList")?.Element("Element")?.Elements()?.Count());
            Assert.AreEqual("0", xml.Element("ElementList")?.Element("Element")?.Element("Child")?.Value);

            element.Poke(xml);
            Assert.AreEqual(1, xml.Element("ElementList")?.Elements()?.Count());
        }

        [TestMethod]
        public void TestException()
        {
            var xml = new XElement("Root");
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new ElementList<Element>("Element", xml, null);
            });
        }

        class TestElement : Element
        {
            public TestElement(XElement? parent) : base(parent, "Element") { }

            public int? Child
            {
                get => GetContent<int>();
                set => SetContent(value);
            }
        }

        [TestMethod]
        public void TestConstruction2()
        {
            var xml = new XElement("Root",
                new XElement("ElementList", new object[]
                {
                    new XElement("Element", new XElement("Child", "0")),
                    new XElement("Element", new XElement("Child", "1")),
                    new XElement("Element", new XElement("Child", "2")),
                    new XElement("Element", new XElement("Child", "3")),
                    new XElement("Element", new XElement("Child", "4")),
                }));

            var element = new ElementList<TestElement>(xml, "ElementList");

            Assert.AreEqual(5, element.Count);
            Assert.AreEqual(0, element[0].Child);
            Assert.AreEqual(1, element[1].Child);
            Assert.AreEqual(2, element[2].Child);
            Assert.AreEqual(3, element[3].Child);
            Assert.AreEqual(4, element[4].Child);

            Assert.AreEqual(1, xml.Elements().Count());
            Assert.AreEqual(0, xml.Element("ElementList")?.Elements()?.Count());
        }

        [TestMethod]
        public void TestAdditionalElement()
        {
            var xml = new XElement("Root",
                new XElement("ElementList", new object[]
                {
                    new XElement("Element", new XElement("Child", "0")),
                    new XElement("OtherElement1"),
                    new XElement("Element", new XElement("Child", "1")),
                    new XElement("Element", new XElement("Child", "2")),
                    new XElement("OtherElement2"),
                    new XElement("Element", new XElement("Child", "3")),
                    new XElement("Element", new XElement("Child", "4")),
                }));

            var element = new ElementList<Element>("Element", xml, "ElementList");

            Assert.IsFalse(element.IsReadOnly);
            Assert.AreEqual(5, element.Count);
            Assert.AreEqual(2, element.XElement?.Elements().Count());
            Assert.IsNotNull(element.XElement?.Element("OtherElement1"));
            Assert.IsNotNull(element.XElement?.Element("OtherElement2"));

            element.Poke(xml);

        }
    }
}
