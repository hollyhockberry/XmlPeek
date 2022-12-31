// Copyright (c) 2022 Inaba (@hollyhockberry)
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System.Xml.Linq;

namespace XmlPeek.Extensions
{
    internal static class XElementExtensions
    {
        static T? Convert<T>(string? str)
        {
            if (str == null)
            {
                return default;
            }
            var type = typeof(T);

            if (type == typeof(int))
            {
                return (T)(object)int.Parse(str);
            }
            if (type == typeof(double))
            {
                return (T)(object)double.Parse(str);
            }
            if (type == typeof(float))
            {
                return (T)(object)float.Parse(str);
            }
            if (type == typeof(bool))
            {
                return (T)(object)bool.Parse(str);
            }
            if (type == typeof(string))
            {
                return (T)(object)str;
            }

            return default;
        }

        public static T? Value<T>(this XElement? element) => Convert<T>(element?.Value);

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
