using System.Runtime.CompilerServices;
using System.Xml.Linq;
using XmlPeek.Extensions;

namespace XmlPeek
{
    public static class ElementExtensions
    {
        public static T? GetContent<T>(this Element element, [CallerMemberName] string? elementName = default)
        {
            if (elementName == null)
            {
                throw new ArgumentNullException(nameof(elementName));
            }
            return (element.XElement?.Element(elementName)).Value<T>();
        }

        public static void SetContent<T>(this Element element, T? value, [CallerMemberName] string? elementName = default)
        {
            if (elementName == null)
            {
                throw new ArgumentNullException(nameof(elementName));
            }
            var e = element.XElement?.Element(elementName);
            if (e == null)
            {
                e = new XElement(elementName);
                element.ValidXElement.Add(e);
            }
            e.Value = $"{value}";
        }
    }
}
