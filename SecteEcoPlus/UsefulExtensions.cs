using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecteEcoPlus
{
    public static class UsefulExtensions
    {
        public static T Default<T>(this T? value, T def = default(T)) where T : struct
        {
            return value.GetValueOrDefault(def);
        }

        public static T DefaultRef<T>(this T value, T def) where T : class
        {
            return value.Equals(default(T)) ? def : value;
        }
    }
}
