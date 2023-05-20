using System;
using System.Collections.Generic;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static T StringToEnum<T>(string enum_str)
        {
            var r = (T)Enum.Parse(typeof(T), enum_str, true);
            return r;

        }

        public static bool StringToEnum<T>(string enum_str, out T enum_)
        {
            enum_ = default(T);
            try
            {
                var r = (T)Enum.Parse(typeof(T), enum_str, true);
                enum_ = r;
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
