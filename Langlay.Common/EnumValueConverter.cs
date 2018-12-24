﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Product.Common {
    public class EnumValueConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return null;
            else
                return GetValues(value.GetType());
        }

        private static readonly IDictionary<Type, object[]> Cache = new Dictionary<Type, object[]>();

        private static object[] GetValues(Type type) {
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum");

            if (!Cache.TryGetValue(type, out var values)) {
                values = type.GetFields()
                    .Where(x => x.IsLiteral)
                    .Select(x => new EnumItem {
                        Key = x.GetValue(null),
                        Text = x
                            .GetCustomAttributes(true)
                            .OfType<DisplayAttribute>()
                            .FirstOrDefault()
                            .GetValueOrDefault(y => y.GetName(), x.GetValue(null).ToString())
                    })
                    .ToArray();
                Cache[type] = values;
            }
            return values;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}