using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMDAPI.Helpers
{
    public static class Utils
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static bool? ToNullableBool(this string s)
        {
            bool i;
            if (Boolean.TryParse(s, out i)) return i;
            return null;
        }
    }
}