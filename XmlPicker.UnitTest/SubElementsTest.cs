// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Xml.Linq;

namespace XmlPicker.UnitTest
{
    [TestClass]
    public class SubElementsTest
    {
        class TestElement : Element
        {
            public TestElement(XElement? parent) : base(parent, "TestElement") { }

            TestSubElement? _SubElement;

            public TestSubElement SubElement => _SubElement ??= new(XElement);

            public string? Child11
            {
                get => GetContent<string?>();
                set => SetContent(value);
            }

            public string? Child12
            {
                get => GetContent<string?>();
                set => SetContent(value);
            }
        }

        class TestSubElement : Element
        {
            public TestSubElement(XElement? parent) : base(parent, "TestSubElement") { }

            public string? Child21
            {
                get => GetContent<string>();
                set => SetContent(value);
            }

            public string? Child22
            {
                get => GetContent<string>();
                set => SetContent(value);
            }
        }

        [TestMethod]
        public void TestConstruction()
        {
            var xml = new XElement("Root",
                new XElement("TestElement", new object[]
                {
                    new XElement("TestSubElement", new object[]
                    {
                        new XElement("Child21", "Text21"),
                        new XElement("Child22", "Text22"),
                    }),
                    new XElement("Child11", "Text11"),
                    new XElement("Child12", "Text12"),
                }));

            var element = new TestElement(xml);

            Assert.AreEqual("Text11", element.Child11);
            Assert.AreEqual("Text12", element.Child12);
            //Assert.AreEqual("Text21", element.SubElement.Child21);
            //Assert.AreEqual("Text22", element.SubElement.Child22);
            Assert.AreEqual(2, element.XElement?.Element("TestSubElement")?.Elements()?.Count());
            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(0, xml.Element("TestElement")?.Elements()?.Count());

            element.Poke(xml);

            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(3, xml.Element("TestElement")?.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("TestElement")?.Element("TestSubElement")?.Elements()?.Count());
            Assert.AreEqual("Text21", xml.Element("TestElement")?.Element("TestSubElement")?.Element("Child21")?.Value);
            Assert.AreEqual("Text22", xml.Element("TestElement")?.Element("TestSubElement")?.Element("Child22")?.Value);
            Assert.AreEqual("Text11", xml.Element("TestElement")?.Element("Child11")?.Value);
            Assert.AreEqual("Text12", xml.Element("TestElement")?.Element("Child12")?.Value);
        }

        [TestMethod]
        public void TestConstructionAndTouch()
        {
            var xml = new XElement("Root",
                new XElement("TestElement", new object[]
                {
                    new XElement("TestSubElement", new object[]
                    {
                        new XElement("Child21", "Text21"),
                        new XElement("Child22", "Text22"),
                    }),
                    new XElement("Child11", "Text11"),
                    new XElement("Child12", "Text12"),
                }));

            var element = new TestElement(xml);

            Assert.AreEqual("Text21", element.SubElement.Child21);
            Assert.AreEqual("Text22", element.SubElement.Child22);
            Assert.AreEqual(0, element.XElement?.Element("TestSubElement")?.Elements()?.Count());

            element.Poke(xml);

            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(3, xml.Element("TestElement")?.Elements()?.Count());
            Assert.AreEqual(2, xml.Element("TestElement")?.Element("TestSubElement")?.Elements()?.Count());
            Assert.AreEqual("Text21", xml.Element("TestElement")?.Element("TestSubElement")?.Element("Child21")?.Value);
            Assert.AreEqual("Text22", xml.Element("TestElement")?.Element("TestSubElement")?.Element("Child22")?.Value);
            Assert.AreEqual("Text11", xml.Element("TestElement")?.Element("Child11")?.Value);
            Assert.AreEqual("Text12", xml.Element("TestElement")?.Element("Child12")?.Value);
        }

        [TestMethod]
        public void TestEmptyConstruction()
        {
            var xml = new XElement("Root",
                new XElement("TestElement", new object[]
                {
                    new XElement("Child11", ""),
                    new XElement("Child12"),
                }));

            var element = new TestElement(xml);

            Assert.AreEqual("", element.Child11);
            //Assert.IsNull(element.Child12);
            Assert.AreEqual(1, xml.Elements()?.Count());
            Assert.AreEqual(0, xml.Element("TestElement")?.Elements()?.Count());

            element.Child11 = null;
            element.Child12 = "";

            element.Poke(xml);

            var text =
@"<Root>
  <TestElement>
    <Child11 />
    <Child12></Child12>
  </TestElement>
</Root>";
            Assert.AreEqual(text, xml.ToString());
        }

    }
}
