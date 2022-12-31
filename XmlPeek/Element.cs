using System.Xml.Linq;

namespace XmlPeek
{
    public class Element
    {
        readonly string Name;

        XElement? _XElement;

        public XElement? XElement => _XElement;

        public XElement ValidXElement => _XElement ??= new(Name);

        public Element(XElement? parent, string name)
        {
            Name = name;

            if (parent != null)
            {
                var element = parent.Element(Name);
                if (element != null)
                {
                    _XElement = new(element);
                    element.RemoveAll();
                }
            }
        }

        public virtual void Poke(XElement parent)
        {
            if (XElement == null)
            {
                return;
            }

            var element = parent.Element(Name);
            if (element == null)
            {
                element = new(Name);
                parent.Add(element);
            }
            element.RemoveAll();
            element.Add(XElement?.Elements());
        }
    }
}
