// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace XmlPicker.Extensions
{
    internal static class XElementExtensions
    {
        static T? Convert<T>(string? str)
        {
            if (str == null)
            {
                return default;
            }
            try
            {
                var type = typeof(T);

                if (type == typeof(int))
                {
                    return (T)(object)int.Parse(str);
                }
                if (type == typeof(int?))
                {
                    return (T?)(object)int.Parse(str);
                }
                if (type == typeof(double))
                {
                    return (T)(object)double.Parse(str);
                }
                if (type == typeof(double?))
                {
                    return (T?)(object)double.Parse(str);
                }
                if (type == typeof(float))
                {
                    return (T)(object)float.Parse(str);
                }
                if (type == typeof(float?))
                {
                    return (T?)(object)float.Parse(str);
                }
                if (type == typeof(bool))
                {
                    return (T)(object)bool.Parse(str);
                }
                if (type == typeof(bool?))
                {
                    return (T?)(object)bool.Parse(str);
                }
                if (type == typeof(string))
                {
                    return (T)(object)str;
                }
            }
            catch (FormatException)
            {
                return default;
            }

            throw new NotImplementedException("Unsupported type");
        }

        public static T? Value<T>(this XElement? element) => Convert<T>(element?.Value);

        public static T? Attribute<T>(this XElement? element, [CallerMemberName] string? name = default)
        {
            if (name is null)
            {
                throw new NotImplementedException();                
            }
            return Convert<T>(element?.Attribute(name)?.Value);
        }

        public static IEnumerable<T>? Values<T>(this XElement? element)
        {
            var values = element?.Elements()?.Select(e => e.Value<T>());
            if (values?.Any(v => v == null) == true)
            {
                throw new NotImplementedException();
            }
            return values?.OfType<T>();
        }
    }
}
