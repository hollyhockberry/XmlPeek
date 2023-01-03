// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Xml.Linq;

namespace XmlPeek.UnitTest
{
    [TestClass]
    public class ValueListTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            var xml = new XElement("Root",
                new XElement("ValueList", new object[]
                {
                    new XElement("Item", "0"),
                    new XElement("Item", "1"),
                    new XElement("Item", "2"),
                    new XElement("Item", "3"),
                    new XElement("Item", "4"),
                }));

            var element = new ValueList<int>(xml, "Item", "ValueList");

            Assert.IsFalse(element.IsReadOnly);
            Assert.AreEqual(5, element.Count);
            Assert.AreEqual(0, element[0]);
            Assert.AreEqual(1, element[1]);
            Assert.AreEqual(2, element[2]);
            Assert.AreEqual(3, element[3]);
            Assert.AreEqual(4, element[4]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => element[-1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => element[5]);

            Assert.AreEqual(0, xml.Element("ValueList")?.Elements()?.Count());

            element.Poke(xml);

            Assert.AreEqual(5, xml.Element("ValueList")?.Elements()?.Count());
            Assert.AreEqual("01234", string.Join("", xml.Element("ValueList")?.Elements()?.Select(e => e.Value)!));
            Assert.AreEqual(1, xml.Element("ValueList")?.Elements()?.Select(e => e.Name).Distinct().Count());
            Assert.AreEqual("Item", xml.Element("ValueList")?.Elements()?.Select(e => e.Name).Distinct().First());
        }

        [TestMethod]
        public void TestEmpty()
        {
            var xml = new XElement("Root", new XElement("ValueList"));
            var element = new ValueList<int>(xml, "Item", "ValueList");
            Assert.AreEqual(0, element.Count);
            element.Poke(xml);
            Assert.AreEqual(0, xml.Element("ValueList")?.Elements()?.Count());

            element.Add(100);
            Assert.AreEqual(1, element.Count);
            element.Poke(xml);
            Assert.AreEqual(1, xml.Element("ValueList")?.Elements()?.Count());
            Assert.AreEqual("100", xml.Element("ValueList")?.Element("Item")?.Value);
        }

        [TestMethod]
        public void TestException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var xml = new XElement("Root");
                _ = new ValueList<int>(xml, "Item", null);
            });

            Assert.ThrowsException<Exception>(() =>
            {
                var xml = new XElement("Root",
                    new XElement("ValueList", new object[]
                    {
                        new XElement("Item", "0"),
                        new XElement("Item", "1"),
                        new XElement("OtherElement", new XElement("Other", "0")),
                        new XElement("Item", "2"),
                        new XElement("Item", "3"),
                        new XElement("Item", "4"),
                    }));
                _ = new ValueList<int>(xml, "Item", "ValueList");
            });
        }
    }
}
