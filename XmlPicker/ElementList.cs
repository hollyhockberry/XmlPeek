// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using XmlPicker.Attributes;

namespace XmlPicker
{
    public class ElementList<T> : Element, IList<T> where T : Element
    {
        List<T>? _List;

        public ElementList(string itemName, XElement? parent, [CallerMemberName] string? name = default) : base(parent, name!)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var ctor = typeof(T).GetConstructor(new Type[] { typeof(XElement), typeof(string) });
            if (ctor == null)
            {
                throw new Exception($"Type {typeof(T).Name} has no available constructor.");
            }
            _List = XElement?.Elements()
                .Select(e => ctor.Invoke(new object[] { new XElement("Root", e), itemName }))
                .OfType<T>()
                .Where(e => e.XElement?.Name == e.Name)
                .ToList();
            if (_List?.Any(e => e.XElement == null) == true)
            {
                throw new Exception($"{name} contains invalid elements.");
            }
            if (_List == null)
            {
                return;
            }
            foreach(var o in _List.Select(e => e.Name).Distinct())
            {
                XElement?.Elements(o).Remove();
            }
        }

        public ElementList(XElement? parent, [CallerMemberName] string? name = default) : base(parent, name!)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var ctor = typeof(T).GetConstructor(new Type[] { typeof(XElement) });
            if (ctor == null)
            {
                throw new Exception($"Type {typeof(T).Name} has no available constructor.");
            }
            _List = XElement?.Elements()
                .Select(e => ctor.Invoke(new object[] { new XElement("Root", e) }))
                .OfType<T>()
                .Where(e => e.XElement?.Name == e.Name)
                .ToList();
            if (_List?.Any(e => e.XElement == null) == true)
            {
                throw new Exception($"{name} contains invalid elements.");
            }
            if (_List == null)
            {
                return;
            }
            foreach (var o in _List.Select(e => e.Name).Distinct())
            {
                XElement?.Elements(o).Remove();
            }
        }

        [IgnoreElement]
        public T this[int index]
        {
            get => List![index];
            set => List![index] = value;
        }

        public List<T>? List => _List;

        List<T> AssuredList => _List ??= new();

        public int Count => List?.Count ?? 0;

        public bool IsReadOnly => false;

        public void Add(T item) => AssuredList.Add(item);

        public void Clear() => List?.Clear();

        public bool Contains(T item) => List?.Contains(item) == true;

        public void CopyTo(T[] array, int arrayIndex) => List?.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
        {
            if (List != null)
            {
                foreach (var i in List)
                {
                    yield return i;
                }
            }
        }

        public int IndexOf(T item) => List?.IndexOf(item) ?? -1;

        public void Insert(int index, T item) => AssuredList.Insert(index, item);

        public bool Remove(T item) => List?.Remove(item) == true;

        public void RemoveAt(int index) => List?.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override void Poke(XElement parent)
        {
            if (List != null)
            {
                var dummy = new XElement("Root");
                foreach (var e in List)
                {
                    e.Poke(dummy);
                }
                foreach(var n in List.Select(e => e.Name).Distinct())
                {
                    ValidXElement.Elements(n)?.Remove();
                }
                ValidXElement.Add(List.Select(e => e.XElement));
            }
            base.Poke(parent);
        }
    }
}
