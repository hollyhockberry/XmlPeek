// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using XmlPeek.Extensions;

namespace XmlPeek
{
    public class ValueList<T> : Element, IList<T>
    {
        readonly string ItemName;

        public ValueList(XElement? parent, string itemName, [CallerMemberName] string? name = default) : base(parent, name!)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            ItemName = itemName;
        }

        public T this[int index]
        {
            get
            {
                var values = XElement?.Values<T>()?.ToArray();
                if (values == null)
                {
                    throw new NotImplementedException();
                }
                return values[index];
            }
            set
            {
                var e = XElement?.Elements()?.Skip(index).First();
                if (e == null)
                {
                    throw new NotImplementedException();
                }
                e.Value = $"{value}";
            }
        }

        public int Count => XElement?.Elements()?.Count() ?? 0;

        public bool IsReadOnly => false;

        XElement CreateElement(T value) => new(ItemName, $"{value}");

        public void Add(T item) => ValidXElement.Add(CreateElement(item));

        public void Clear() => XElement?.RemoveAll();
        public bool Contains(T item) => XElement?.Values<T>()?.ToArray()?.Contains(item) == true;

        public void CopyTo(T[] array, int arrayIndex) => XElement?.Values<T>()?.ToArray()?.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
        {
            var elements = XElement?.Elements();
            if (elements != null)
            {
                foreach (var e in elements)
                {
                    var v = e.Value<T>();
                    if (v is null)
                    {
                        throw new NotImplementedException();
                    }
                    yield return v;
                }
            }
        }

        public int IndexOf(T item) => XElement?.Values<T>()?.ToList()?.IndexOf(item) ?? -1;

        public void Insert(int index, T item)
        {
            var count = XElement?.Elements()?.Count();
            if (count == index || (XElement == null && index == 0))
            {
                Add(item);
                return;
            }
            count ??= -1;
            if (index < 0 || count < index)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            XElement?.Elements().Skip(index).First()?.AddBeforeSelf(CreateElement(item));
        }

        public bool Remove(T item)
        {
            var e = XElement?.Elements().FirstOrDefault(e => e?.Value<T>()?.Equals(item) == true);
            if (e == null)
            {
                return false;
            }
            e?.Remove();
            return true;
        }

        public void RemoveAt(int index)
        {
            var count = XElement?.Elements()?.Count();
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            XElement?.Elements().Skip(index).First()?.Remove();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
