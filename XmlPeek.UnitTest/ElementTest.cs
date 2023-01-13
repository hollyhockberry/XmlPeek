// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Xml.Linq;

namespace XmlPeek.UnitTest
{
    [TestClass]
    public class ElementTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            var xml = new XElement("Root", new object[]
            {
                new XElement("Element", new object[]
                {
                    new XElement("Child1", "Text1"),
                    new XElement("Child2", "Text2"),
                }),
                new XElement("OtherElement", new object[]
                {
                    new XElement("Child3", "Text3"),
                    new XElement("Child4", "Text4"),
                }),
            });

            var element = new Element(xml, "Element");

            Assert.AreEqual(2, element.XElement?.Elements()?.Count());
            Assert.AreEqual("Text1", element.XElement?.Element("Child1")?.Value);
            Assert.AreEqual("Text2", element.XElement?.Element("Child2")?.Value);

            Assert.AreEqual(2, xml.Elements()?.Count());
            Assert.AreEqual(0, xml.Element("Element")?.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);

            element.Poke(xml);

            Assert.AreEqual(2, xml.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("Element")?.Elements()?.Count());
            Assert.AreEqual("Text1", xml.Element("Element")?.Element("Child1")?.Value);
            Assert.AreEqual("Text2", xml.Element("Element")?.Element("Child2")?.Value);
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);
        }

        [TestMethod]
        public void TestEmptyConstruction()
        {
            var xml = new XElement("Root", new object[]
            {
                new XElement("Element"),
                new XElement("OtherElement", new object[]
                {
                    new XElement("Child3", "Text3"),
                    new XElement("Child4", "Text4"),
                }),
            });

            var element = new Element(xml, "Element");

            Assert.AreEqual(0, element.XElement?.Elements()?.Count());

            Assert.AreEqual(2, xml.Elements()?.Count());
            Assert.AreEqual(0, xml.Element("Element")?.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);

            element.Poke(xml);

            Assert.AreEqual(2, xml.Elements()?.Count());
            Assert.AreEqual(0, xml.Element("Element")?.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);
        }

        [TestMethod]
        public void TestNullConstruction()
        {
            var xml = new XElement("Root", new object[]
            {
                new XElement("OtherElement", new object[]
                {
                    new XElement("Child3", "Text3"),
                    new XElement("Child4", "Text4"),
                }),
            });

            var element = new Element(xml, "Element");

            Assert.IsNull(element.XElement);
            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);

            element.Poke(xml);

            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);
        }

        [TestMethod]
        public void TestNullConstructionAndModification()
        {
            var xml = new XElement("Root", new object[]
            {
                new XElement("OtherElement", new object[]
                {
                    new XElement("Child3", "Text3"),
                    new XElement("Child4", "Text4"),
                }),
            });

            var element = new Element(xml, "Element");

            element.ValidXElement.Add(new XElement("Child1", "Text1"));
            element.ValidXElement.Add(new XElement("Child2", "Text2"));

            element.Poke(xml);

            Assert.AreEqual(2, xml.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("Element")?.Elements()?.Count());
            Assert.AreEqual("Text1", xml.Element("Element")?.Element("Child1")?.Value);
            Assert.AreEqual("Text2", xml.Element("Element")?.Element("Child2")?.Value);
            Assert.AreEqual(2, xml.Element("OtherElement")?.Elements()?.Count());
            Assert.AreEqual("Text3", xml.Element("OtherElement")?.Element("Child3")?.Value);
            Assert.AreEqual("Text4", xml.Element("OtherElement")?.Element("Child4")?.Value);
        }

        class TestElement : Element
        {
            public int? Integer1 => GetContent<int?>();

            public int? Integer2 => GetContent<int?>();

            public int? Integer3 => GetContent<int?>();

            public double? Double1 => GetContent<double?>();

            public double? Double2 => GetContent<double?>();

            public double? Double3 => GetContent<double?>();

            public float? Float1 => GetContent<float?>();

            public float? Float2 => GetContent<float?>();

            public float? Float3 => GetContent<float?>();

            public bool? Boolean1 => GetContent<bool?>();

            public bool? Boolean2 => GetContent<bool?>();

            public bool? Boolean3 => GetContent<bool?>();

            public string? String1 => GetContent<string>();

            public string? String2 => GetContent<string>();

            public string? String3 => GetContent<string>();

            public TestElement(XElement? parent) : base(parent, "Element") { }
        }

        [TestMethod]
        public void TestContent()
        {
            var xml = new XElement("Root", 
                new XElement("Element", new object[]
                {
                    new XElement("Integer1", "12345"),
                    new XElement("Integer2"),
                    new XElement("Double1", "1.2345"),
                    new XElement("Double2"),
                    new XElement("Float1", "3.14"),
                    new XElement("Float2"),
                    new XElement("Boolean1", "True"),
                    new XElement("Boolean2"),
                    new XElement("String1", "Text"),
                    new XElement("String2"),
                }));

            var element = new TestElement(xml);
            Assert.AreEqual(12345, element.Integer1);
            Assert.IsNull(element.Integer2);
            Assert.IsNull(element.Integer3);
            Assert.AreEqual(1.2345, element.Double1);
            Assert.IsNull(element.Double2);
            Assert.IsNull(element.Double3);
            Assert.AreEqual(3.14f, element.Float1);
            Assert.IsNull(element.Float2);
            Assert.IsNull(element.Float3);
            Assert.IsTrue(element.Boolean1);
            Assert.IsNull(element.Boolean2);
            Assert.IsNull(element.Boolean3);
            Assert.AreEqual("Text", element.String1);
            Assert.AreEqual("", element.String2);
            Assert.IsNull(element.String3);
        }
    }
}