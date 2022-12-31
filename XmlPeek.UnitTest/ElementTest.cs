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
    }
}