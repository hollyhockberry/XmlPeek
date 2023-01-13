// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using XmlPeek.Attributes;
using XmlPeek.Extensions;

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

        public Element(string name)
        {
            Name = name;
        }

        IEnumerable<Element> GetSubElements()
        {
            static bool IsInherit(Type type, Type parent)
            {
                Type? t = type;
                while (t != null && t != typeof(object))
                {
                    if (t == parent)
                    {
                        return true;
                    }
                    t = t.BaseType;
                }
                return false;
            }

            return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => IsInherit(p.PropertyType, typeof(Element)))
                .Where(p => p.GetCustomAttribute<IgnoreElementAttribute>() == null)
                .Select(p => p.GetValue(this))
                .OfType<Element>();
        }

        public virtual void Poke(XElement parent)
        {
            var subElements = GetSubElements();
            if (subElements.Any(e => e.XElement != null))
            {
                _ = ValidXElement;
            }
            if (XElement == null)
            {
                return;
            }
            foreach (var e in subElements)
            {
                e.Poke(XElement);
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

        public T? GetContent<T>([CallerMemberName] string? elementName = default)
        {
            if (elementName == null)
            {
                throw new ArgumentNullException(nameof(elementName));
            }
            return (XElement?.Element(elementName)).Value<T>();
        }

        public void SetContent<T>(T? value, [CallerMemberName] string? elementName = default)
        {
            if (elementName == null)
            {
                throw new ArgumentNullException(nameof(elementName));
            }
            var e = XElement?.Element(elementName);
            if (e == null)
            {
                e = new XElement(elementName);
                ValidXElement.Add(e);
            }
            e.Value = $"{value}";
        }
    }
}
